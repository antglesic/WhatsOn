using GrabaUIPackage.Components.Common.EventArgs;
using Microsoft.AspNetCore.Components;
using WhatsOn.Web.Services.ShowApiService;
using WhatsOn.Web.Services.ShowApiService.Models;
using WhatsOn.Web.Services.ShowApiService.Records;

namespace WhatsOn.WebApplication.Pages.Shows;

public partial class Shows : ComponentBase
{
	private readonly int _pageSize = 20;
	private int _pageNumber = 1;
	private readonly int[] _pageSizeOptions = [20];
	private int _rowCount;
	private bool _isLoading;
	private string _query = string.Empty;
	private bool _includeAdult;
	private Show[]? _shows;

	[Inject]
	IShowApiClient ShowService { get; set; } = default!;

	[Inject]
	NavigationManager NavigationManager { get; set; } = default!;

	protected override async Task OnInitializedAsync()
	{
		await LoadShows();
		await base.OnInitializedAsync();
	}

	private async Task HandleSearch()
	{
		_pageNumber = 1;
		await LoadShows();
	}

	private async Task LoadShows()
	{
		_isLoading = true;

		GetShowsRequest request = new()
		{
			Query = _query,
			PageNumber = _pageNumber,
			IncludeAdult = _includeAdult
		};

		GetShowsResponse response = await ShowService.GetShowsAsync(request);

		if (response is { Success: true }
			&& response.Shows is { Data: not null })
		{
			_shows = [.. response.Shows.Data];
			_rowCount = response.Shows.TotalItemCount;
			await InvokeAsync(StateHasChanged);
		}

		_isLoading = false;
	}

	private async Task OnPageNumberChange(PageChangedEventArgs args)
	{
		_pageNumber = args.CurrentPage;
		await LoadShows();
	}

	private Task View(Show show)
	{
		NavigationManager.NavigateTo($"/shows/view/{show.Id}");
		return Task.CompletedTask;
	}
}
