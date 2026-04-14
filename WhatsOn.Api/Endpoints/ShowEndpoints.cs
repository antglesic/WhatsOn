using WhatsOn.Domain;
using WhatsOn.Service.ShowService;
using WhatsOn.Service.ShowService.Models;
using WhatsOn.Service.ShowService.Records;

namespace WhatsOn.Api.Endpoints
{
	public static class ShowEndpoints
	{
		public static void MapShowEndpoints(this IEndpointRouteBuilder app)
		{
			RouteGroupBuilder group = app.MapGroup("/shows")
				.WithTags("Shows");

			group.MapGet("/getshows", GetShows)
				.WithName("Show endpoint GetShows")
				.WithSummary("Search tv shows")
				.WithDescription("Retrieves the tv show list according to the query")
				.CacheOutput("ShowSearch")
				.Produces<GetShowsResponse>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status400BadRequest)
				.Produces(StatusCodes.Status500InternalServerError);

			group.MapGet("/showdetails/{id}", GetShowDetails)
				.WithName("Show endpoint GetShowDetails")
				.WithSummary("Search tv show details by Id")
				.WithDescription("Retrieves the tv show details retrieved by show id")
				.CacheOutput("ShowDetails")
				.Produces<GetShowDetailsResponse>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status400BadRequest)
				.Produces(StatusCodes.Status500InternalServerError);
		}

		private static async Task<IResult> GetShows(
			IShowService showService,
			int? pageNumber,
			bool? includeAdult,
			CancellationToken cancellationToken,
			string? query = null)
		{
			GetShowsRequest request = new()
			{
				Query = query ?? string.Empty,
				PageNumber = pageNumber,
				IncludeAdult = includeAdult
			};

			GetShowsResponse response = await showService.GetShows(request, cancellationToken);

			if (!response.Success)
				return Results.BadRequest(response.ErrorResult);

			return Results.Ok(response);
		}

		private static async Task<IResult> GetShowDetails(
			int id,
			IShowService showService,
			CancellationToken cancellationToken)
		{
			GetShowDetailsRequest request = new()
			{
				Id = id
			};

			GetShowDetailsResponse response = await showService.GetShowDetails(request, cancellationToken);

			if (!response.Success)
				return Results.BadRequest(response.ErrorResult);

			return Results.Ok(response);
		}
	}
}
