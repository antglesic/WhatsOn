using Microsoft.Extensions.Logging;
using System.Text.Json;
using WhatsOn.Service.MovieService.Models;
using WhatsOn.Service.MovieService.Records;

namespace WhatsOn.Service.MovieService
{
	public class MovieService(
		HttpClient httpClient,
		ILogger<MovieService> logger) : IMovieService
	{
		private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
		public async Task<GetMoviesResponse> GetMovies(GetMoviesRequest request, CancellationToken cancellationToken)
		{
			GetMoviesResponse response = new()
			{
				Request = request
			};

			try
			{
				var url = request.Query switch
				{
					{ Length: > 0 } query => $"search/movie?{BuildQueryString(new Dictionary<string, string?>
					{
						["query"] = Uri.EscapeDataString(query),
						["page"] = request.PageNumber.ToString(),
						["include_adult"] = request.IncludeAdult.ToString()
					})}",
					_ => $"discover/movie?page={request.PageNumber}"
				};

				HttpResponseMessage responseMessage = await httpClient.GetAsync(url, cancellationToken);
				if (responseMessage is { IsSuccessStatusCode: true })
				{
					Stream responseStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
					if (responseStream is { Length: > 0 })
					{
						var movies = await JsonSerializer.DeserializeAsync<MovieSearchResponse>(responseStream, jsonSerializerOptions, cancellationToken);

						if (movies is { TotalResults: > 0 })
						{
							response.Movies = movies.ToPagedResult();
						}
						else
						{
							response.Message = "No movies were found!";
						}
					}
				}

				response.Success = true;
			}
			catch (HttpRequestException ex)
			{
				logger.LogError(ex, "HTTP error fetching movies for query={Query}", request.Query);
				response.Message = "Failed to reach the movie database. Please try again later.";
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Unexpected error fetching movies for query={Query}", request.Query);
				response.Message = "An unexpected error occurred.";
			}

			return response;
		}

		public async Task<GetMovieDetailsResponse> GetMovieDetails(GetMovieDetailsRequest request, CancellationToken cancellationToken)
		{
			GetMovieDetailsResponse response = new()
			{
				Request = request
			};

			try
			{
				var url = $"movie/{request.Id}?append_to_response=videos";

				HttpResponseMessage responseMessage = await httpClient.GetAsync(url, cancellationToken);
				if (responseMessage is { IsSuccessStatusCode: true })
				{
					Stream responseStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
					if (responseStream is { Length: > 0 })
					{
						var movie = await JsonSerializer.DeserializeAsync<MovieDetailResponse>(responseStream, jsonSerializerOptions, cancellationToken);

						if (movie is not null)
						{
							response.MovieDetails = movie;
						}
						else
						{
							response.Message = $"No movie was found for Id: {request.Id}!";
						}
					}
				}

				response.Success = true;
			}
			catch (HttpRequestException ex)
			{
				logger.LogError(ex, "HTTP error fetching movie details for MovieId={Id}", request.Id);
				response.Message = "Failed to reach the movie database. Please try again later.";
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Unexpected error fetching movie details for MovieId={Id}", request.Id);
				response.Message = "An unexpected error occurred.";
			}

			return response;
		}

		private static string BuildQueryString(Dictionary<string, string?> queryParams)
		{
			return string.Join("&", queryParams
				.Where(x => !string.IsNullOrEmpty(x.Value))
				.Select(x => $"{x.Key}={x.Value}"));
		}
	}
}
