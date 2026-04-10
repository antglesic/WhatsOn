using WhatsOn.Service.MovieService.Models;

namespace WhatsOn.Service.MovieService
{
	public interface IMovieService
	{
		Task<GetMoviesResponse> GetMovies(GetMoviesRequest request);
	}
}
