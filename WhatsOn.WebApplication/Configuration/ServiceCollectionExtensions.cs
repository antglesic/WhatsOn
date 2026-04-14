using Microsoft.Extensions.Options;
using WhatsOn.Web.Services.Common.ApiClientBase;
using WhatsOn.Web.Services.MovieApiService;
using WhatsOn.Web.Services.ShowApiService;

namespace WhatsOn.WebApplication.Configuration;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddWhatsOnApiClients(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddOptions<WhatsOnApiOptions>()
			.Bind(configuration.GetSection(WhatsOnApiOptions.SectionName))
			.Validate(options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _), $"{WhatsOnApiOptions.SectionName}:BaseUrl must be an absolute URI.");

		services.AddHttpClient<IMovieApiClient, MovieApiClient>(ConfigureHttpClient);
		services.AddHttpClient<IShowApiClient, ShowApiClient>(ConfigureHttpClient);

		return services;
	}

	private static void ConfigureHttpClient(IServiceProvider serviceProvider, HttpClient client)
	{
		WhatsOnApiOptions options = serviceProvider.GetRequiredService<IOptions<WhatsOnApiOptions>>().Value;
		client.BaseAddress = new Uri(options.BaseUrl, UriKind.Absolute);
	}
}
