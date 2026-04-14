# WhatsOn

WhatsOn is a .NET-based API solution for searching and retrieving movie and TV show information, including metadata and trailers. It integrates with The Movie Database (TMDb) API to provide paginated search results and detailed information for films and series.

## Key Features

- **Movie and Show Search**: Search for movies and TV shows with pagination support
- **Detailed Metadata**: Retrieve comprehensive details including trailers and videos
- **Scalable Architecture**: Built with ASP.NET Core Minimal APIs, Redis caching, and resilience patterns
- **Performance Optimized**: Includes output caching, response compression, and rate limiting

## Project Structure

- `WhatsOn.Api`: Main API project with endpoints for movies and shows
- `WhatsOn.Service`: Business logic layer handling external API calls
- `WhatsOn.Domain`: Shared domain models and DTOs
- `Orchestration.*`: Hosting and service defaults for deployment
- `WhatsOn.WebApplication`: Blazor web application for frontend interaction
- `WhatsOn.Web.Services`: Client services for API communication

## Getting Started

1. Set up your TMDB API key using `set-tmdb-access-token.cmd`
2. Run the solution using the AppHost project for local development
3. Access the API endpoints at `/movies` and `/shows`

For detailed setup and architecture information, see [Solution.md](Solution.md).