using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using WhatsOn.Domain;
using WhatsOn.Service.MovieService.Models;
using WhatsOn.Service.MovieService.Records;

namespace WhatsOn.Web.Services.Tests;

public class MovieApiClientTests
{
	[Fact]
	public async Task GetMoviesAsync_BuildsExpectedQueryString()
	{
		StubHttpMessageHandler handler = new((_, _) => Task.FromResult(CreateJsonResponse(new GetMoviesResponse
		{
			Success = true,
			Movies = new PagedResult<Movie>
			{
				PageNumber = 2,
				TotalPages = 5,
				Data = [new Movie { Id = 1, Title = "Dune" }]
			}
		})));

		MovieApiClient client = CreateClient(handler);

		GetMoviesResponse response = await client.GetMoviesAsync(new GetMoviesRequest
		{
			Query = "star wars",
			PageNumber = 2,
			IncludeAdult = true
		});

		Assert.Equal("/movies/getmovies?query=star%20wars&pageNumber=2&includeAdult=True", handler.LastRequest?.RequestUri?.PathAndQuery);
		Assert.True(response.Success);
		Assert.Single(response.Movies.Data);
	}

	[Fact]
	public async Task GetMovieDetailsAsync_UsesMovieDetailsRoute()
	{
		StubHttpMessageHandler handler = new((_, _) => Task.FromResult(CreateJsonResponse(new GetMovieDetailsResponse
		{
			Success = true,
			MovieDetails = new MovieDetailResponse
			{
				Id = 99,
				Title = "Dune"
			}
		})));

		MovieApiClient client = CreateClient(handler);

		GetMovieDetailsResponse response = await client.GetMovieDetailsAsync(new GetMovieDetailsRequest { Id = 99 });

		Assert.Equal("/movies/moviedetails/99", handler.LastRequest?.RequestUri?.PathAndQuery);
		Assert.True(response.Success);
		Assert.Equal(99, response.MovieDetails.Id);
	}

	[Fact]
	public async Task GetMoviesAsync_MapsBadRequestErrors()
	{
		StubHttpMessageHandler handler = new((_, _) => Task.FromResult(CreateJsonResponse(
			new { Message = "Query is required.", ErrorId = Guid.NewGuid() },
			HttpStatusCode.BadRequest)));

		MovieApiClient client = CreateClient(handler);

		GetMoviesResponse response = await client.GetMoviesAsync(new GetMoviesRequest { Query = string.Empty });

		Assert.False(response.Success);
		Assert.Equal("Query is required.", response.Message);
		Assert.NotNull(response.Request);
	}

	private static MovieApiClient CreateClient(HttpMessageHandler handler)
	{
		return new MovieApiClient(new HttpClient(handler)
		{
			BaseAddress = new Uri("https://api.whats-on.test/")
		});
	}

	private static HttpResponseMessage CreateJsonResponse<T>(T payload, HttpStatusCode statusCode = HttpStatusCode.OK)
	{
		return new HttpResponseMessage(statusCode)
		{
			Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json)
		};
	}
}
