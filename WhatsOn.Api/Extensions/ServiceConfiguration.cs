using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using WhatsOn.Service.Common;
using WhatsOn.Service.Common.Configuration;
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
				.ConfigureAppSettings(configuration)
				.AddCorsPolicy(configuration)
				.AddRateLimiter()
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
			services.Configure<ExternalServicesSettings>(configuration.GetSection(nameof(ExternalServicesSettings)));
			services.Configure<CorsPolicySettings>(configuration.GetSection(CorsPolicySettings.SectionName));

			return services;
		}

		private static IServiceCollection AddCustomOutputCaching(this IServiceCollection services)
		{
			services.AddOutputCache(options =>
			{
				options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromMinutes(15)));

				options.AddPolicy("MovieSearch", policy => policy.Expire(TimeSpan.FromMinutes(15)));
				options.AddPolicy("MovieDetails", policy => policy.Expire(TimeSpan.FromHours(24)));
				options.AddPolicy("ShowSearch", policy => policy.Expire(TimeSpan.FromMinutes(15)));
				options.AddPolicy("ShowDetails", policy => policy.Expire(TimeSpan.FromHours(24)));
			});

			return services;
		}

		private static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
		{
			CorsPolicySettings corsPolicySettings = configuration.GetSection(CorsPolicySettings.SectionName).Get<CorsPolicySettings>()
				?? throw new InvalidOperationException($"{CorsPolicySettings.SectionName} configuration is required.");

			string[] allowedOrigins = corsPolicySettings.AllowedOrigins;

			if (allowedOrigins.Length == 0)
			{
				throw new InvalidOperationException($"{CorsPolicySettings.SectionName}:AllowedOrigins must contain at least one origin.");
			}

			services.AddCors(options =>
			{
				options.AddPolicy("WhatsOnWebClient", policy =>
				{
					policy.WithOrigins(allowedOrigins)
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
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

			services
				.AddHttpClient<IShowService, ShowService>((serviceProvider, client) =>
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
