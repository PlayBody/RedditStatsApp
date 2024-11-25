
# Reddit Statistics Application

This application listens to specified subreddits and collects statistics in near real time. It tracks the posts with the most upvotes and users with the most posts. The application continuously requests data from the Reddit API and reports the statistics to the console.

## Table of Contents

- [Features](#features)
- [Requirements](#requirements)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Running Tests](#running-tests)

## Features

- Monitors specified subreddits in real time.
- Tracks:
  - Posts with the most upvotes.
  - Users with the most posts.
- Periodically reports statistics to the console.
- Handles errors and rate limits from the Reddit API.
- Unit tests for core functionality.

## Requirements

- .NET 8.0
- An active Reddit account to obtain API credentials.

  First please set API redirect url as follows
![alt text](./Help/image.png)

## Installation

1. Clone the repository:
  ```bash
  git clone https://github.com/playbody/RedditStatsApp.git
  cd RedditStatsApp
  ```
2. Open the solution in Visual Studio or your preferred IDE.
3. Restore the NuGet packages:
  ```bash
  dotnet restore
  ```
## Configuration
Before running the application, you need to configure your Reddit API credentials.

1. Open App.config (or appsettings.json if you are using .NET Core) and set the following values:

  ```xml
  <?xml version="1.0" encoding="utf-8" ?>
  <configuration>
    <appSettings>
        <add key="appId" value="YOUR_REDDIT_APP_ID" />
        <add key="appSecret" value="YOUR_REDDIT_APP_SECRET" />
        <add key="subreddits" value="technology,todayilearned" />
        <add key="waitingMs" value="60000" />
    </appSettings>
  </configuration>
  ```
Replace `YOUR_REDDIT_APP_ID` and `YOUR_REDDIT_APP_SECRET` with your actual Reddit API credentials. You can create an application in your Reddit account settings to obtain these credentials.
## Usage

1. Build and run the application:

  ```bash
  dotnet run
  ```
2. The application will open a browser window for Reddit authorization. Follow the prompts to authorize the application.

3. Once authorized, the application will start monitoring the specified subreddits and report statistics to the console.

4. Press any key to exit the application.

### API Endpoints
The application exposes the following RESTful API endpoints:

- GET /api/statistics/top-post: Returns the post with the most upvotes.
- GET /api/statistics/top-user: Returns the user with the most posts.
- GET /api/subreddit: Lists all currently monitored subreddits.
- POST /api/subreddit: Adds a subreddit to be monitored. Requires a subreddit name in the request body.
### Swagger UI
Access Swagger UI: Navigate to http://localhost:5000 (or your configured port) to view the Swagger UI. This interface allows you to explore and test the API endpoints interactively.

## Running Tests
To run the unit tests for the application:

1. Navigate to the test project directory:

  ```bash
  cd Tests
  ```
2. Run the tests using the following command:

  ```bash
  dotnet test
  ```
You can also run tests directly from Visual Studio using the Test Explorer.

## Architecture
1. Services: 

Process reddit in background mode.
Manages in-memory statistics for posts and users.
Handles Reddit API interactions and monitors subreddits.

2. Controllers: 

Provides RESTful API endpoints for accessing statistics and managing subreddits.

3. Models: 

Defines data structures for Post and User.

4. Interfaces:

Defines contracts for services to ensure loose coupling.
Considerations