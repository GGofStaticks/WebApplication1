using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Enteties;
using WebApplication1.Database;
using AppContext = WebApplication1.Database.AppContext;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {
        private readonly AppContext _context;
        private readonly IBackgroundJobClient _jobClient;

        // Конструктор для внедрения зависимостей
        public MyController(AppContext context, IBackgroundJobClient jobClient)
        {
            _context = context;
            _jobClient = jobClient;
        }

        // Обрабатывает HTTP GET запросы по маршруту /api/Stats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stats>>> GetStats()
        {
            // Используем AppContext для доступа к данным и асинхронно получаем их все
            var statsList = await _context.Stats.ToListAsync();

            // Возвращаем данные в формате JSON с кодом состояния 200 OK
            return Ok(statsList);
        }

        // Запуск фоновой задачи Hangfire
        // Обрабатывает HTTP POST запросы по маршруту /api/MyController/trigger-job
        [HttpPost("trigger-job")]
        public IActionResult TriggerHangfireJob(string jobData)
        {
            if (string.IsNullOrEmpty(jobData))
            {
                return BadRequest("Параметр jobData не может быть пустым.");
            }

            // IBackgroundJobClient для постановки задачи в очередь
            _jobClient.Enqueue<IBackgroundJobService>(service => service.ProcessAsync(jobData));

            return Ok($"Фоновая задача с данными '{jobData}' успешно поставлена в очередь Hangfire.");
        }
    }

}

