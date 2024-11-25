using Microsoft.Extensions.Hosting;
using RedditStatsApp.Interfaces;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RedditStatsApp.Services
{
    public class SubredditMonitoringService : IHostedService
    {
        private readonly IRedditService _redditService;
        private readonly IStatisticsService _statisticsService;

        public SubredditMonitoringService(IRedditService redditService, IStatisticsService statisticsService)
        {
            _redditService = redditService;
            _statisticsService = statisticsService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Opening browser for Reddit authorization...");
            await _redditService.AuthorizeAsync();

            // Get subreddits from configuration
            string[] subreddits = ConfigurationManager.AppSettings["subreddits"]?.Split(',') ?? ["technology"];

            var monitoringTasks = subreddits.Select(subreddit =>
                _redditService.MonitorSubredditAsync(subreddit.Trim(), _statisticsService)).ToArray();

            await Task.WhenAll(monitoringTasks);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Implement any cleanup or shutdown logic here
            return Task.CompletedTask;
        }
    }
}
