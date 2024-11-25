using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedditStatsApp.Interfaces;
using RedditStatsApp.Services;
using System.Configuration;

namespace RedditStatsApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IStatisticsService, StatisticsService>();
                    services.AddSingleton<IRedditService, RedditService>(provider =>
                    {
                        var appId = ConfigurationManager.AppSettings["appId"] ?? "";
                        var appSecret = ConfigurationManager.AppSettings["appSecret"] ?? "";
                        var redirectUri = ConfigurationManager.AppSettings["redirectUri"] ?? "";
                        var waitingMs = int.Parse(ConfigurationManager.AppSettings["waitingMs"] ?? "60000");
                        return new RedditService(appId, appSecret, redirectUri, waitingMs);
                    });
                    services.AddHostedService<SubredditMonitoringService>();
                });
    }
}