using RedditStatsApp.Models;

namespace RedditStatsApp.Interfaces
{
    public interface IStatisticsService
    {
        IOrderedEnumerable<User> UserPostCounts { get; }
        IOrderedEnumerable<Post> PostUpvotes { get; }

        void UpdateStatistics(string author, string postId, string title, int upvotes);
        void ReportStatistics();
    }
}
