using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Reddit.Things;
using RedditStatsApp.Interfaces;

namespace RedditStatsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("top-post")]
        public IActionResult GetTopPost()
        {
            var topPost = _statisticsService.PostUpvotes.FirstOrDefault();
            return Ok(topPost);
        }

        [HttpGet("top-user")]
        public IActionResult GetTopUser()
        {
            var topUser = _statisticsService.UserPostCounts.FirstOrDefault();
            return Ok(topUser);
        }
    }
}
