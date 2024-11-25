using System.Diagnostics;
using Reddit;
using Reddit.AuthTokenRetriever;
using Reddit.AuthTokenRetriever.EventArgs;
using RedditStatsApp.Interfaces;

namespace RedditStatsApp.Services
{
    public class RedditService(string appId, string appSecret, string redirectUri, int waitingMs) : IRedditService
    {
        private readonly string _appId = appId;
        private readonly string _appSecret = appSecret;
        private readonly string _redirectUri = redirectUri;
        private readonly int _waitingMs = waitingMs;
        private string _accessToken = "";
        private string _refreshToken = "";

        public async Task AuthorizeAsync()
        {
            var authTokenRetrieverLib = new AuthTokenRetrieverLib(_appId, 8080, null, _redirectUri, _appSecret);
            var tcs = new TaskCompletionSource<bool>();
            authTokenRetrieverLib.AuthSuccess += (sender, e) =>
            {
                OnAuthSuccess(sender, e);
                tcs.SetResult(true);
            };

            authTokenRetrieverLib.AwaitCallback();
            OpenBrowser(authTokenRetrieverLib.AuthURL());
            await tcs.Task;
            authTokenRetrieverLib.StopListening();
        }

        private void OnAuthSuccess(object? sender, AuthSuccessEventArgs e)
        {
            Console.WriteLine("Authorization successful!");
            _accessToken = e.AccessToken;
            _refreshToken = e.RefreshToken;
        }

        public async Task MonitorSubredditAsync(string subredditName, IStatisticsService statisticsService)
        {
            if (string.IsNullOrEmpty(_accessToken) || string.IsNullOrEmpty(_refreshToken))
            {
                throw new InvalidOperationException("Authorization is required before monitoring a subreddit.");
            }

            var redditClient = new RedditClient(_appId, _refreshToken, _appSecret, _accessToken);
            var subreddit = redditClient.Subreddit(subredditName).About();

            string after = "";
            Console.WriteLine($"Monitoring subreddit: {subreddit.Name}");
            while (true)
            {
                try
                {
                    var posts = subreddit.Posts.GetNew(after: after);
                    foreach (var post in posts)
                    {
                        statisticsService.UpdateStatistics(post.Author, post.Id, post.Title, post.UpVotes);
                    }
                    statisticsService.ReportStatistics();
                    after = posts[posts.Count - 1].Fullname;

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error monitoring subreddit {subredditName}: {ex.Message}");
                }
                await Task.Delay(_waitingMs);
            }
        }

        private static void OpenBrowser(string authUrl)
        {
            try
            {
                Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open browser: {ex.Message}");
            }
        }

        public void SetAccessToken(string v)
        {
            _accessToken = v;
        }

        public void SetRefreshToken(string v)
        {
            _refreshToken = v;
        }
    }
}
