using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication1.Database;
using AppContext = WebApplication1.Database.AppContext;

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<YandexMetrikaOptions>(Configuration.GetSection("YandexMetrika"));

            services.AddHttpClient();
            services.AddHangfire(configuration =>
            configuration.UseMemoryStorage());
            services.AddHangfireServer();

            services.AddScoped<IBackgroundJobService, YandexDataFetch>();

            services.AddRazorPages();
            services.AddControllersWithViews();

            services.AddScoped<IDashboardAuthorizationFilter, MyAuthorizationFilter>();

            services.AddDbContext<AppContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHttpClient<IYandexApiService, YandexApiService>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
        {

            RecurringJob.AddOrUpdate<IBackgroundJobService>(
                "yandex-data-fetch-job",
                service => service.ProcessAsync("Автоматический запуск данных при старте"),
                Cron.Daily); // Каждый день


            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter(app.ApplicationServices.GetRequiredService<IConfiguration>()) }
            });

            app.UseHangfireDashboard();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
