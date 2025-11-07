using Newtonsoft.Json;

namespace WebApplication1
{
    public class ApiResponseDto
    {
        [JsonProperty("data")]
        public ApiDataDto[] data { get; set; }
    }

    public class ApiDataDto
    {
        [JsonProperty("dimensions")]
        public DimensionDto[] dimensions { get; set; }

        [JsonProperty("metrics")]
        public double[] metrics { get; set; }
    }

    public class DimensionDto
    {
        [JsonProperty("name")]
        public string name { get; set; }
    }

    public class VisitsData
    {
        public DateTime date { get; set; }
        public int visitsCount { get; set; }
        public int dau { get; set; }
        public int mau { get; set; }
        public int newUsers { get; set; }
        public int returningUsers { get; set; }
        public int newUsersBounce { get; set; }
        public int newUsersNoBounce { get; set; }
        public int returningUsersBounce { get; set; }
        public int returningUsersNoBounce { get; set; }
        public string trafficSource { get; set; }
    }
}

