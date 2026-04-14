using WhatsOn.Web.Services.MovieApiService.Models;

namespace WhatsOn.Web.Services.MovieApiService;

public interface IMovieApiClient
{
	Task<GetMoviesResponse> GetMoviesAsync(GetMoviesRequest request, CancellationToken cancellationToken = default);
	Task<GetMovieDetailsResponse> GetMovieDetailsAsync(GetMovieDetailsRequest request, CancellationToken cancellationToken = default);
}
