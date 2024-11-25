using Microsoft.AspNetCore.Mvc;
using RedditStatsApp.Interfaces;
using RedditStatsApp.Services;


namespace RedditStatsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubredditController : ControllerBase
    {
        private readonly IRedditService _redditService;
        private readonly List<string> _subreddits = new List<string>();

        public SubredditController(IRedditService redditService)
        {
            _redditService = redditService;
        }

        [HttpGet]
        public IActionResult GetSubreddits()
        {
            return Ok(_subreddits);
        }

        [HttpPost]
        public IActionResult AddSubreddit([Microsoft.AspNetCore.Mvc.FromBody] string subreddit)
        {
            if (_subreddits.Contains(subreddit))
            {
                return BadRequest("Subreddit is already being monitored.");
            }

            _subreddits.Add(subreddit);
            _redditService.MonitorSubredditAsync(subreddit, new StatisticsService());
            return Ok($"Started monitoring subreddit: {subreddit}");
        }

        //[HttpDelete]
        //public IActionResult RemoveSubreddit([Microsoft.AspNetCore.Mvc.FromBody] string subreddit)
        //{
        //    if (!_subreddits.Contains(subreddit))
        //    {
        //        return NotFound("Subreddit is not being monitored.");
        //    }

        //    _subreddits.Remove(subreddit);
        //    // Note: You would need to implement logic to stop monitoring the subreddit
        //    return Ok($"Stopped monitoring subreddit: {subreddit}");
        //}
    }
}
