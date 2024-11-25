namespace RedditStatsApp.Tests
{
    internal interface IRedditClient
    {
        void GetNewPosts(string subredditName, string v);
        object Subreddit(string subredditName);
    }
}