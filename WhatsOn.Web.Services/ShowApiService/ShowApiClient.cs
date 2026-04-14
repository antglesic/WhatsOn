using System.Globalization;
using WhatsOn.Web.Services.ShowApiService.Models;

namespace WhatsOn.Web.Services.ShowApiService;

public sealed class ShowApiClient(HttpClient httpClient) : WhatsOnApiClientBase(httpClient), IShowApiClient
{
	public Task<GetShowsResponse> GetShowsAsync(GetShowsRequest request, CancellationToken cancellationToken = default)
	{
		string requestUri = BuildRelativeUri(
			"shows/getshows",
			new Dictionary<string, string?>
			{
				["query"] = request.Query,
				["pageNumber"] = request.PageNumber?.ToString(CultureInfo.InvariantCulture),
				["includeAdult"] = request.IncludeAdult?.ToString()
			});

		return SendGetAsync<GetShowsResponse, GetShowsRequest>(requestUri, request, cancellationToken);
	}

	public Task<GetShowDetailsResponse> GetShowDetailsAsync(GetShowDetailsRequest request, CancellationToken cancellationToken = default)
	{
		string requestUri = $"shows/showdetails/{request.Id.ToString(CultureInfo.InvariantCulture)}";
		return SendGetAsync<GetShowDetailsResponse, GetShowDetailsRequest>(requestUri, request, cancellationToken);
	}
}
