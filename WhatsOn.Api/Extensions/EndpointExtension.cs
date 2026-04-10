using WhatsOn.Api.Endpoints;

namespace WhatsOn.Api.Extensions
{
	public static class EndpointExtension
	{
		public static void MapEndpoints(this IEndpointRouteBuilder app)
		{
			app.MapMovieEndpoints();
			app.MapShowEndpoints();
		}
	}
}
