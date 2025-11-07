using Microsoft.EntityFrameworkCore;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

CreateHostBuilder(args).Build().Run();
static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

