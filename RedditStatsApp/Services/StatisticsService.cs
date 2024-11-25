using System;
using System.Collections.Concurrent;
using System.Linq;
using RedditStatsApp.Interfaces;
using RedditStatsApp.Models;

namespace RedditStatsApp.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();
        private readonly ConcurrentDictionary<string, Post> _posts = new ConcurrentDictionary<string, Post>();

        public IOrderedEnumerable<User> UserPostCounts
        {
            get { return _users.Values.OrderByDescending(user => user.PostCount); }
        }

        public IOrderedEnumerable<Post> PostUpvotes
        {
            get { return _posts.Values.OrderByDescending(post => post.Upvotes); }
        }

        public void UpdateStatistics(string author, string postId, string title, int upvotes)
        {
            // Update user post count
            _users.AddOrUpdate(author,
                new User { Username = author, PostCount = 1 },
                (key, existingUser) =>
                {
                    existingUser.PostCount++;
                    return existingUser;
                });

            // Update or add post
            _posts.AddOrUpdate(postId,
                new Post { Id = postId, Author = author, Title = title, Upvotes = upvotes },
                (key, existingPost) =>
                {
                    existingPost.Upvotes = Math.Max(existingPost.Upvotes, upvotes);
                    return existingPost;
                });
        }

        public void ReportStatistics()
        {
            Console.WriteLine("---- Statistics ----");

            if (_posts.Any())
            {
                var topPost = PostUpvotes.First();
                Console.WriteLine($"Top Post Upvotes: ID: {topPost.Id} by {topPost.Author} with {topPost.Upvotes} upvotes");
            }

            if (_users.Any())
            {
                var topUser = UserPostCounts.First();
                Console.WriteLine($"Top User: {topUser.Username} with {topUser.PostCount} posts");
            }

            Console.WriteLine("--------------------");
        }
    }
}
