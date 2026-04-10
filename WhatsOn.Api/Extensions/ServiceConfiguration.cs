using WhatsOn.Service.MovieService;

namespace WhatsOn.Api.Extensions
{
	public static class ServiceConfiguration
	{
		public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddApplicationServices();
			services.AddCustomOutputCaching();
			services.AddConfiguration(configuration);
		}

		private static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddTransient<IMovieService, MovieService>();

			return services;
		}

		private static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			//services.Configure<ToDoApiSettings>(configuration.GetSection(nameof(ToDoApiSettings)));
			return services;
		}

		private static IServiceCollection AddCustomOutputCaching(this IServiceCollection services)
		{
			services.AddOutputCache(options =>
			{
				options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromMinutes(15)));

				// You can add named policies for different cache durations
				options.AddPolicy("search", policy => policy.Expire(TimeSpan.FromMinutes(15)));
				options.AddPolicy("details", policy => policy.Expire(TimeSpan.FromHours(24)));
				options.AddPolicy("trailers", policy => policy.Expire(TimeSpan.FromHours(48)));
			});

			return services;
		}
	}
}
