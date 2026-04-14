using Microsoft.AspNetCore.Components;
using WhatsOn.Web.Services.ShowApiService;
using WhatsOn.Web.Services.ShowApiService.Models;
using WhatsOn.Web.Services.ShowApiService.Records;

namespace WhatsOn.WebApplication.Pages.Shows;

public partial class ShowView : ComponentBase
{
	private bool _isLoading = true;
	private string? _errorMessage;

	[Parameter]
	public int Id { get; set; }

	[Inject]
	IShowApiClient ShowService { get; set; } = default!;

	private ShowDetails? ShowDetails;
	private string PageTitleText
	{
		get
		{
			return string.IsNullOrWhiteSpace(ShowDetails?.Name)
		? "Show Details"
		: $"{ShowDetails.Name} Details";
		}
	}

	protected override async Task OnParametersSetAsync()
	{
		_isLoading = true;
		_errorMessage = null;

		GetShowDetailsResponse response = await ShowService.GetShowDetailsAsync(new GetShowDetailsRequest
		{
			Id = Id
		});

		if (response is { Success: true })
		{
			ShowDetails = response.ShowDetails;
		}
		else
		{
			ShowDetails = new ShowDetails();
			_errorMessage = "Show details could not be loaded.";
		}

		_isLoading = false;
	}

	protected static string DisplayOrFallback(string? value)
	{
		return string.IsNullOrWhiteSpace(value) ? "N/A" : value;
	}

	protected static string DisplayDateOrFallback(DateOnly? value)
	{
		return value.HasValue ? value.Value.ToString("yyyy-MM-dd") : "N/A";
	}

	protected static string DisplayDateOrFallback(DateOnly value)
	{
		return value == default ? "N/A" : value.ToString("yyyy-MM-dd");
	}
}
