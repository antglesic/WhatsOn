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

			group.MapGet("/getmovies", GetMovies)
				.WithName("Movie endpoint GetMovies")
				.WithSummary("Search movies")
				.WithDescription("Retrieves the movies list according to the query")
				.CacheOutput("MovieSearch")
				.Produces<PagedResult<Movie>>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status404NotFound)
				.Produces(StatusCodes.Status500InternalServerError);

			group.MapGet("/moviedetails/{id}", GetMovieDetails)
				.WithName("Movie endpoint GetMovieDetails")
				.WithSummary("Search movie details by Id")
				.WithDescription("Retrieves the movie details retrieved by movie id")
				.CacheOutput("MovieDetails")
				.Produces<MovieDetailResponse>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status404NotFound)
				.Produces(StatusCodes.Status500InternalServerError);
		}

		private static async Task<IResult> GetMovies(
			IMovieService movieService,
			int? pageNumber,
			bool? includeAdult,
			CancellationToken cancellationToken,
			string? query = null)
		{
			GetMoviesRequest request = new()
			{
				Query = query ?? string.Empty,
				PageNumber = pageNumber,
				IncludeAdult = includeAdult
			};

			GetMoviesResponse response = await movieService.GetMovies(request, cancellationToken);

			if (!response.Success)
				return Results.BadRequest(response.ErrorResult);

			return Results.Ok(new { response.Movies, response.Message });
		}

		private static async Task<IResult> GetMovieDetails(
			int id,
			IMovieService movieService,
			CancellationToken cancellationToken)
		{
			GetMovieDetailsRequest request = new()
			{
				Id = id
			};

			GetMovieDetailsResponse response = await movieService.GetMovieDetails(request, cancellationToken);

			if (!response.Success)
				return Results.BadRequest(response.ErrorResult);

			return Results.Ok(response);
		}
	}
}
