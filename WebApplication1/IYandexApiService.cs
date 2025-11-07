namespace WebApplication1
{
    public interface IYandexApiService
    {
        Task<ApiResponseDto> GetDataAsync(
            string counterId,
            string token,
            string metrics,
            string dimensions = "ym:s:date",
            string group = "day"
        );
    }
}