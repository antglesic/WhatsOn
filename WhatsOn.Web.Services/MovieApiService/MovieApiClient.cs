using Microsoft.Extensions.Logging;
using System.Globalization;
using WhatsOn.Web.Services.Common.ApiClientBase;
using WhatsOn.Web.Services.MovieApiService.Models;

namespace WhatsOn.Web.Services.MovieApiService;

public sealed class MovieApiClient(
	HttpClient httpClient,
	ILogger<MovieApiClient> logger)
	: WhatsOnApiClientBase(httpClient, logger), IMovieApiClient
{
	public Task<GetMoviesResponse> GetMoviesAsync(GetMoviesRequest request)
	{
		string requestUri = BuildRelativeUri(
			"movies/getmovies",
			new Dictionary<string, string>
			{
				["query"] = request.Query,
				["pageNumber"] = request.PageNumber?.ToString(CultureInfo.InvariantCulture),
				["includeAdult"] = request.IncludeAdult?.ToString()
			});

		return ClientAppRequest<GetMoviesResponse>(requestUri, HttpMethod.Get);
	}

	public Task<GetMovieDetailsResponse> GetMovieDetailsAsync(GetMovieDetailsRequest request)
	{
		string requestUri = $"movies/moviedetails/{request.Id.ToString(CultureInfo.InvariantCulture)}";
		return ClientAppRequest<GetMovieDetailsResponse>(requestUri, HttpMethod.Get);
	}
}
