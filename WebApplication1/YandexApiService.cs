using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplication1;

public class YandexApiService : IYandexApiService
{
    private readonly HttpClient _httpClient;

    public YandexApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponseDto> GetDataAsync(string counterId, string token, string metrics, string dimensions = "ym:s:date", string group = "day")
    {
        var date1 = "2025-07-22";
        var date2 = DateTime.Today.ToString("yyyy-MM-dd");

        var url = $"https://api-metrika.yandex.net/stat/v1/data" +
                  $"?ids={counterId}" +
                  $"&metrics={metrics}" +
                  $"&dimensions={dimensions}" +
                  $"&sort=ym:s:date" +
                  $"&date1={date1}" +
                  $"&date2={date2}" +
                  $"&limit=10000" +
                  $"&accuracy=full" +
                  $"&group={group}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Authorization", $"OAuth {token}");

        var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"ошибка api: {(int)response.StatusCode} {response.ReasonPhrase}, ответ: {content}");
        }

        return JsonConvert.DeserializeObject<ApiResponseDto>(content);
    }
}

