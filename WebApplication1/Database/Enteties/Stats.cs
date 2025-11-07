using System.Data;

namespace WebApplication1.Database.Enteties
{
    public class Stats
    {
        public int id { get; set; }
        public DateTime date { get; set; } // дата
        public int visitsCount { get; set; } // визиты (visits)
        public int dau { get; set; } // пользователи за день
        public int mau { get; set; } // пользователи за месяц
        public int newUsers { get; set; } // новые
        public int returningUsers { get; set; } // вернувшиеся
        public int newUsersBounce { get; set; } // новые — отказ
        public int newUsersNoBounce { get; set; } // новые — не отказ
        public int returningUsersBounce { get; set; } // вернувшиеся — отказ
        public int returningUsersNoBounce { get; set; } // вернувшиеся — не отказ
        public string? trafficSource { get; set; } // источник трафика
    }
}