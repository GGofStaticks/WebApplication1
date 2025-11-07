using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Enteties;

namespace WebApplication1.Database
{
    public class AppContext : DbContext
    {
        public DbSet<Stats> Stats { get; set; } = null!;

        public AppContext(DbContextOptions<AppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
            {
                property.SetColumnType("timestamp without time zone");
            }
        }
    }
}
