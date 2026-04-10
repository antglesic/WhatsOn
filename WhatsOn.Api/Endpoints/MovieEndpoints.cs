using WhatsOn.Domain;
using WhatsOn.Service.MovieService;
using WhatsOn.Service.MovieService.Models;
using WhatsOn.Service.MovieService.Records;

namespace WhatsOn.Api.Endpoints
{
	public static class MovieEndpoints
	{
		public static void MapMovieEndpoints(this IEndpointRouteBuilder app)
		{
			RouteGroupBuilder group = app.MapGroup("/movies")
				.WithTags("Movies");

			group.MapGet("/health", GetApiHealth)
				.WithName("Movie endpoint GetHealth")
				.WithSummary("Get Health")
				.WithDescription("Retrieves the health of the api.")
				.Produces(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status404NotFound)
				.Produces(StatusCodes.Status500InternalServerError);

			group.MapGet("/getmovies", GetMovies)
				.WithName("Movie endpoint GetMovies")
				.WithSummary("Search movies")
				.WithDescription("Retrieves the movies list according to the query")
				.Produces<PagedResult<Movie>>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status404NotFound)
				.Produces(StatusCodes.Status500InternalServerError);

		}

		private static async Task<IResult> GetApiHealth(CancellationToken cancellationToken)
		{
			var retval = new
			{
				Status = "Healthy",
				Timestamp = DateTime.UtcNow
			};

			await Task.Delay(100, cancellationToken); // Simulate some async work

			return Results.Ok(retval);
		}

		private static async Task<IResult> GetMovies(
			IMovieService movieService,
			string query,
			CancellationToken cancellationToken)
		{
			GetMoviesRequest request = new()
			{
				Query = query
			};

			GetMoviesResponse response = await movieService.GetMovies(request);

			if (!response.Success)
				return Results.BadRequest(response.ErrorResult);

			return Results.Ok(new { response.Movies, response.Message });
		}
	}
}
