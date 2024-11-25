namespace RedditStatsApp.Models
{
    public class Post
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public int Upvotes { get; set; }
    }
}
