using System.Globalization;
using WhatsOn.Web.Services.MovieApiService.Models;

namespace WhatsOn.Web.Services.MovieApiService;

public sealed class MovieApiClient(HttpClient httpClient) : WhatsOnApiClientBase(httpClient), IMovieApiClient
{
	public Task<GetMoviesResponse> GetMoviesAsync(GetMoviesRequest request, CancellationToken cancellationToken = default)
	{
		string requestUri = BuildRelativeUri(
			"movies/getmovies",
			new Dictionary<string, string?>
			{
				["query"] = request.Query,
				["pageNumber"] = request.PageNumber?.ToString(CultureInfo.InvariantCulture),
				["includeAdult"] = request.IncludeAdult?.ToString()
			});

		return SendGetAsync<GetMoviesResponse, GetMoviesRequest>(requestUri, request, cancellationToken);
	}

	public Task<GetMovieDetailsResponse> GetMovieDetailsAsync(GetMovieDetailsRequest request, CancellationToken cancellationToken = default)
	{
		string requestUri = $"movies/moviedetails/{request.Id.ToString(CultureInfo.InvariantCulture)}";
		return SendGetAsync<GetMovieDetailsResponse, GetMovieDetailsRequest>(requestUri, request, cancellationToken);
	}
}
