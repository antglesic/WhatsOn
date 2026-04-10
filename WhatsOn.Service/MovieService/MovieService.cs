using Microsoft.Extensions.Logging;
using WhatsOn.Service.MovieService.Models;

namespace WhatsOn.Service.MovieService
{
	public class MovieService(ILogger<MovieService> logger) : IMovieService
	{
		public async Task<GetMoviesResponse> GetMovies(GetMoviesRequest request)
		{
			GetMoviesResponse response = new()
			{
				Request = request
			};

			try
			{
				response.Message = "No movies were found!";
				response.Success = true;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "{ExceptionMessage} | An error occured for request: {@Request}",
					ex.Message,
					request);
			}

			return response;
		}
	}
}
