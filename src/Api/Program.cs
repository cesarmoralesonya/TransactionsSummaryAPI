using Infraestructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace PublicApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void SeedDatabase(IHost host)
        {
            var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TransSummaryContext>();
                if(context.Database.EnsureCreated())
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    try
                    {
                        TransSummaryContextSeed.Initialize(context);
                        logger.LogInformation("A database seeding was initialize");
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex, "A db seeding error ocurred");
                    }
                }
            }
        }
    }
}
