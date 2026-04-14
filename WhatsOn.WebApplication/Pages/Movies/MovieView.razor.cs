using Microsoft.AspNetCore.Components;
using WhatsOn.Web.Services.MovieApiService;
using WhatsOn.Web.Services.MovieApiService.Models;
using WhatsOn.Web.Services.MovieApiService.Records;

namespace WhatsOn.WebApplication.Pages.Movies;

public partial class MovieView : ComponentBase
{
	private bool _isLoading = true;
	private string? _errorMessage;

	[Parameter]
	public int Id { get; set; }

	[Inject]
	IMovieApiClient MovieService { get; set; } = default!;

	private MovieDetails? MovieDetails;

	protected string PageTitleText
	{
		get
		{
			return string.IsNullOrWhiteSpace(MovieDetails?.Title)
				? "Movie Details"
				: $"{MovieDetails.Title} Details";
		}
	}

	protected override async Task OnParametersSetAsync()
	{
		_isLoading = true;
		_errorMessage = null;

		GetMovieDetailsResponse response = await MovieService.GetMovieDetailsAsync(new GetMovieDetailsRequest
		{
			Id = Id
		});

		if (response is { Success: true })
		{
			MovieDetails = response.MovieDetails;
		}
		else
		{
			MovieDetails = new MovieDetails();
			_errorMessage = "Movie details could not be loaded.";
		}

		_isLoading = false;
	}

	protected static string DisplayOrFallback(string? value)
	{
		return string.IsNullOrWhiteSpace(value) ? "N/A" : value;
	}

	protected static string DisplayRuntime(int? runtime)
	{
		return runtime is null or <= 0 ? "N/A" : $"{runtime} min";
	}
}
