using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using RedditStatsApp.Services;

namespace RedditStatsApp.Tests
{
    public class StatisticsServiceTests
    {
        [Fact]
        public void UpdateStatistics_IncreasesPostCount()
        {
            // Arrange
            var statisticsService = new StatisticsService();
            string author = "testUser";
            string fullname = "testPost";
            int upvotes = 10;
            string title = "abc";

            // Act
            statisticsService.UpdateStatistics(author, fullname, title, upvotes);
            statisticsService.UpdateStatistics(author, fullname, title, upvotes);

            Assert.NotNull(statisticsService.UserPostCounts);
            var topUser = statisticsService.UserPostCounts.First();
            Assert.Equal(author, topUser.Username);
            Assert.Equal(2, topUser.PostCount);
        }

        [Fact]
        public void UpdateStatistics_RecordsTopPostUpvotes()
        {
            // Arrange
            var statisticsService = new StatisticsService();
            string author = "testUser";
            string fullname1 = "testPost1";
            string fullname2 = "testPost2";
            string title1 = "abc1";
            string title2 = "abc2";
            int upvotes1 = 5;
            int upvotes2 = 10;

            // Act
            statisticsService.UpdateStatistics(author, fullname1, title1, upvotes1);
            statisticsService.UpdateStatistics(author, fullname2, title2, upvotes2);

            Assert.NotNull(statisticsService.PostUpvotes);
            var topPost = statisticsService.PostUpvotes.First();
            Assert.Equal(fullname2, topPost.Id);
            Assert.Equal(upvotes2, topPost.Upvotes);
        }

        [Fact]
        public void ReportStatistics_PrintsCorrectOutput()
        {
            // Arrange
            var statisticsService = new StatisticsService();
            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            statisticsService.UpdateStatistics("user1", "post1", "abc1", 5);
            statisticsService.UpdateStatistics("user2", "post2", "abc2", 10);
            statisticsService.ReportStatistics();

            // Assert
            var output = sw.ToString();
            Assert.Contains(" by user2 with 10 upvotes", output);
            Assert.Contains("user2 with 1 posts", output);
        }
    }
}
