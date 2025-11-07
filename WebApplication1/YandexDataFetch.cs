using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApplication1.Database;
using WebApplication1.Database.Enteties;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Database
{
    public class YandexDataFetch : IBackgroundJobService
    {
        private readonly AppContext _dbContext;
        private readonly ILogger _logger;
        private readonly IYandexApiService _yandexApiService;
        private readonly string _yandexMetrikaToken;
        private readonly string _counterId;

        public YandexDataFetch(
            AppContext dbContext,
            ILogger<YandexDataFetch> logger,
            IConfiguration configuration,
            IYandexApiService yandexApiService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _yandexApiService = yandexApiService;
            _yandexMetrikaToken = configuration["YandexMetrika:Token"];
            _counterId = configuration["YandexMetrika:CounterId"];
        }

        public async Task ProcessAsync(string data)
        {
            // данные: визиты, пользователи, новые, отказы
            var baseData = await _yandexApiService.GetDataAsync(
                _counterId,
                _yandexMetrikaToken,
                "ym:s:visits,ym:s:users,ym:s:newUsers,ym:s:bounceRate",
                "ym:s:date,ym:s:lastTrafficSource",
                "day"
            );

            if (baseData?.data == null || baseData.data.Length == 0)
            {
                _logger.LogWarning("Нет данных от api");
                return;
            }

            // преобразование api ответа
            var dailyList = ProcessApiResponse(baseData);

            // массив Stats для сохранения
            var mergedStats = dailyList.Select(v => new Stats
            {
                date = v.date,
                visitsCount = v.visitsCount,
                dau = v.dau,
                mau = 0,
                newUsers = v.newUsers,
                returningUsers = v.returningUsers,
                newUsersBounce = v.newUsersBounce,
                newUsersNoBounce = v.newUsersNoBounce,
                returningUsersBounce = v.returningUsersBounce,
                returningUsersNoBounce = v.returningUsersNoBounce,
                trafficSource = v.trafficSource
            }).ToArray();

            await SaveVisitsData(mergedStats);
        }

        private VisitsData[] ProcessApiResponse(ApiResponseDto data)
        {
            var list = new List<VisitsData>();

            foreach (var item in data.data)
            {
                var dateStr = item.dimensions?.FirstOrDefault(d => d.name.Contains("-"))?.name;
                var source = item.dimensions?.LastOrDefault()?.name ?? "unknown";

                if (string.IsNullOrEmpty(dateStr) || !DateTime.TryParse(dateStr, out var date))
                    continue;

                var visits = (int)(item.metrics.ElementAtOrDefault(0));
                var users = (int)(item.metrics.ElementAtOrDefault(1));
                var newUsers = (int)(item.metrics.ElementAtOrDefault(2));
                var bounceRate = item.metrics.ElementAtOrDefault(3);

                var returningUsers = users - newUsers;

                var newUsersBounce = (int)Math.Round(newUsers * (bounceRate / 100.0));
                var returningUsersBounce = (int)Math.Round(returningUsers * (bounceRate / 100.0));

                list.Add(new VisitsData
                {
                    date = date,
                    visitsCount = visits,
                    dau = users,
                    newUsers = newUsers,
                    returningUsers = returningUsers,
                    newUsersBounce = newUsersBounce,
                    newUsersNoBounce = newUsers - newUsersBounce,
                    returningUsersBounce = returningUsersBounce,
                    returningUsersNoBounce = returningUsers - returningUsersBounce,
                    trafficSource = source
                });
            }

            // группировка по дате и источнику
            return list
                .GroupBy(v => new { v.date.Date, v.trafficSource })
                .Select(g => new VisitsData
                {
                    date = g.Key.Date,
                    trafficSource = g.Key.trafficSource,
                    visitsCount = g.Sum(x => x.visitsCount),
                    dau = g.Sum(x => x.dau),
                    newUsers = g.Sum(x => x.newUsers),
                    returningUsers = g.Sum(x => x.returningUsers),
                    newUsersBounce = g.Sum(x => x.newUsersBounce),
                    newUsersNoBounce = g.Sum(x => x.newUsersNoBounce),
                    returningUsersBounce = g.Sum(x => x.returningUsersBounce),
                    returningUsersNoBounce = g.Sum(x => x.returningUsersNoBounce)
                })
                .OrderBy(v => v.date)
                .ToArray();
        }

        // сохранение данных в бд и подсчет dau в пределах месяца
        private async Task SaveVisitsData(Stats[] statsData)
        {

            _logger.LogInformation($"начало {statsData.Length} записей, пример: {JsonConvert.SerializeObject(statsData.Take(3))}");

            // сортировка по дате
            var orderedStats = statsData.OrderBy(s => s.date).ToArray();

            // группировка по месяцу
            var groupedByMonth = orderedStats
                .GroupBy(s => new { s.date.Year, s.date.Month })
                .ToDictionary(g => g.Key, g => g.ToList());

            // mau внутри каждого месяца
            foreach (var stat in orderedStats)
            {
                var key = new { stat.date.Year, stat.date.Month };
                var currentMonthData = groupedByMonth[key];

                // mau = сумма dau за месяц
                stat.mau = currentMonthData.Sum(s => s.dau);

                // сохранение или обновление записи
                var existing = await _dbContext.Stats.FirstOrDefaultAsync(
                    s => s.date == stat.date && s.trafficSource == stat.trafficSource);

                if (existing == null)
                    await _dbContext.Stats.AddAsync(stat);
                else
                {
                    existing.visitsCount = stat.visitsCount;
                    existing.dau = stat.dau;
                    existing.mau = stat.mau;
                    existing.newUsers = stat.newUsers;
                    existing.returningUsers = stat.returningUsers;
                    existing.newUsersBounce = stat.newUsersBounce;
                    existing.newUsersNoBounce = stat.newUsersNoBounce;
                    existing.returningUsersBounce = stat.returningUsersBounce;
                    existing.returningUsersNoBounce = stat.returningUsersNoBounce;
                    existing.trafficSource = stat.trafficSource;
                    _dbContext.Stats.Update(existing);
                }
            }

            await _dbContext.SaveChangesAsync();

            var savedStats = await _dbContext.Stats
                .OrderByDescending(s => s.date)
                .Take(5)
                .Select(s => new { s.id, s.date, s.visitsCount, s.dau, s.mau, s.trafficSource })
                .ToListAsync();

            _logger.LogInformation($"сохранение закончено, последние записи: {JsonConvert.SerializeObject(savedStats)}");
        }
    }
}