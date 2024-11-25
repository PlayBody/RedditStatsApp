using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedditStatsApp.Interfaces;
using RedditStatsApp.Services;

namespace RedditStatsApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IStatisticsService, StatisticsService>();
            services.AddSingleton<IRedditService, RedditService>(provider =>
            {
                var appId = Configuration["appId"] ?? "";
                var appSecret = Configuration["appSecret"] ?? "";
                var redirectUri = Configuration["redirectUri"] ?? "";
                var waitingMs = int.Parse(Configuration["waitingMs"] ?? "60000");
                return new RedditService(appId, appSecret, redirectUri, waitingMs);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Reddit Stats API",
                    Version = "v1"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reddit Stats API v1");
                c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
