using System.Threading.Tasks;

namespace RedditStatsApp.Interfaces
{
    public interface IRedditService
    {
        Task AuthorizeAsync();
        Task MonitorSubredditAsync(string subredditName, IStatisticsService statisticsService);
    }
}
