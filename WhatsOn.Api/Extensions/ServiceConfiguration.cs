using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using WhatsOn.Service.Common;
using WhatsOn.Service.MovieService;
using WhatsOn.Service.ShowService;

namespace WhatsOn.Api.Extensions
{
	public static class ServiceConfiguration
	{
		public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddApplicationServices()
				.AddCustomOutputCaching()
				.AddRateLimiter()
				.ConfigureAppSettings(configuration)
				.ConfigureHttpClients();
		}

		private static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddTransient<IMovieService, MovieService>();
			services.AddTransient<IShowService, ShowService>();
			services.AddTransient<TheMovieDbAuthHandler>();

			return services;
		}

		private static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddOptions<ExternalServicesSettings>()
				.Bind(configuration.GetSection(ExternalServicesSettings.SectionName))
				.ValidateDataAnnotations()
				.ValidateOnStart();

			return services;
		}

		private static IServiceCollection AddCustomOutputCaching(this IServiceCollection services)
		{
			services.AddOutputCache(options =>
			{
				options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromMinutes(15)));

				// You can add named policies for different cache durations
				options.AddPolicy("MovieSearch", policy => policy.Expire(TimeSpan.FromMinutes(15)));
				options.AddPolicy("MovieDetails", policy => policy.Expire(TimeSpan.FromHours(24)));
				options.AddPolicy("trailers", policy => policy.Expire(TimeSpan.FromHours(48)));
			});

			return services;
		}

		private static IServiceCollection ConfigureHttpClients(this IServiceCollection services)
		{
			services
				.AddHttpClient<IMovieService, MovieService>((serviceProvider, client) =>
				{
					var settings = serviceProvider
						.GetRequiredService<IOptions<ExternalServicesSettings>>().Value;

					client.BaseAddress = new Uri(settings.TheMovieDbDocumentationApiBaseUrl);
					client.DefaultRequestHeaders.Accept
						.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				})
				.AddHttpMessageHandler<TheMovieDbAuthHandler>()
				.AddStandardResilienceHandler();

			return services;
		}
	}
}
