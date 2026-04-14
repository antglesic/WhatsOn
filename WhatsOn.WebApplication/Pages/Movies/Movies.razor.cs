using GrabaUIPackage.Components.Common.EventArgs;
using Microsoft.AspNetCore.Components;
using WhatsOn.Web.Services.MovieApiService;
using WhatsOn.Web.Services.MovieApiService.Models;
using WhatsOn.Web.Services.MovieApiService.Records;

namespace WhatsOn.WebApplication.Pages.Movies;

public partial class Movies : ComponentBase
{
	private readonly int _pageSize = 20;
	private int _pageNumber = 1;
	private readonly int[] _pageSizeOptions = [20];
	private int _rowCount = 0;
	private bool _isLoading;
	private string _query = string.Empty;
	private bool _includeAdult;
	private Movie[]? _movies;

	[Inject]
	IMovieApiClient MovieService { get; set; } = default!;

	protected override async Task OnInitializedAsync()
	{
		await LoadMovies();
		await base.OnInitializedAsync();
	}

	private async Task HandleSearch()
	{
		_pageNumber = 1;
		await LoadMovies();
	}

	private async Task LoadMovies()
	{
		_isLoading = true;

		GetMoviesRequest request = new()
		{
			Query = _query,
			PageNumber = _pageNumber,
			IncludeAdult = _includeAdult
		};

		GetMoviesResponse response = await MovieService.GetMoviesAsync(request);

		if (response is { Success: true }
			&& response.Movies is { Data: not null })
		{
			_movies = [.. response.Movies.Data];
			_rowCount = response.Movies.TotalItemCount;
			await InvokeAsync(StateHasChanged);
		}

		_isLoading = false;
	}

	private async Task OnPageNumberChange(PageChangedEventArgs args)
	{
		_pageNumber = args.CurrentPage;
		await LoadMovies();
	}
}
