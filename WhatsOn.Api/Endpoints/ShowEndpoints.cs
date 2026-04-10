namespace WhatsOn.Api.Endpoints
{
	public static class ShowEndpoints
	{
		public static void MapShowEndpoints(this IEndpointRouteBuilder app)
		{
			RouteGroupBuilder group = app.MapGroup("/shows")
				.WithTags("Shows");

			group.MapGet("/health", GetApiHealth)
				.WithName("Shop endpoint GetHealth")
				.WithSummary("Get Health")
				.WithDescription("Retrieves the health of the api.")
				.Produces(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status404NotFound)
				.Produces(StatusCodes.Status500InternalServerError)
				.Produces(StatusCodes.Status401Unauthorized);
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
	}
}
