using Moq;
using Xunit;
using RedditStatsApp.Interfaces;
using RedditStatsApp.Services;
using System.Threading.Tasks;
using RedditStatsApp.Models;

namespace RedditStatsApp.Tests
{
    public class RedditServiceTests
    {
        private readonly Mock<IStatisticsService> _mockStatisticsService;
        private readonly RedditService _redditService;


        public RedditServiceTests()
        {
            _mockStatisticsService = new Mock<IStatisticsService>();

            // Initialize the RedditService with mocked dependencies
            _redditService = new RedditService("appId", "appSecret", "http://localhost:8080", 1000);
        }


        [Fact]
        public async Task MonitorSubredditAsync_ShouldThrowExceptionIfNotAuthorized()
        {
            // Arrange
            string subredditName = "testSubreddit";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _redditService.MonitorSubredditAsync(subredditName, _mockStatisticsService.Object)
            );

            Assert.Equal("Authorization is required before monitoring a subreddit.", exception.Message);
        }


    }
}
