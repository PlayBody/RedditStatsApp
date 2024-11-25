using System;
using System.Configuration;
using System.Threading.Tasks;
using RedditStatsApp.Services;
using RedditStatsApp.Interfaces;

string appId = ConfigurationManager.AppSettings["appId"] ?? throw new ArgumentNullException("appId");
string appSecret = ConfigurationManager.AppSettings["appSecret"] ?? throw new ArgumentNullException("appSecret");
string[] subreddits = (ConfigurationManager.AppSettings["subreddits"] ?? "technology").Split(',');

int waitingMs = int.Parse(ConfigurationManager.AppSettings["waitingMs"] ?? "60000");
string redirectUri = "http://localhost:8080";

try
{
    var redditService = new RedditService(appId, appSecret, redirectUri, waitingMs);
    var statisticsService = new StatisticsService();

    Console.WriteLine("Opening browser for Reddit authorization...");
    await redditService.AuthorizeAsync();


    var monitoringTasks = subreddits.Select(subreddit =>
        redditService.MonitorSubredditAsync(subreddit.Trim(), statisticsService)).ToArray();

    await Task.WhenAll(monitoringTasks);
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}

Console.WriteLine("Press any key to exit...");
ConsoleKeyInfo _ = Console.ReadKey();