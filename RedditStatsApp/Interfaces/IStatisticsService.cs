namespace RedditStatsApp.Interfaces
{
    public interface IStatisticsService
    {
        void UpdateStatistics(string author, string postId, string title, int upvotes);
        void ReportStatistics();
    }
}
