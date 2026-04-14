using Microsoft.Extensions.Logging;
using System.Globalization;
using WhatsOn.Web.Services.Common.ApiClientBase;
using WhatsOn.Web.Services.ShowApiService.Models;

namespace WhatsOn.Web.Services.ShowApiService;

public sealed class ShowApiClient(
	HttpClient httpClient,
	ILogger<ShowApiClient> logger)
	: WhatsOnApiClientBase(httpClient, logger), IShowApiClient
{
	public Task<GetShowsResponse> GetShowsAsync(GetShowsRequest request)
	{
		string requestUri = BuildRelativeUri(
			"shows/getshows",
			new Dictionary<string, string>
			{
				["query"] = request.Query,
				["pageNumber"] = request.PageNumber?.ToString(CultureInfo.InvariantCulture),
				["includeAdult"] = request.IncludeAdult?.ToString()
			});

		return ClientAppRequest<GetShowsResponse>(requestUri, HttpMethod.Get);
	}

	public Task<GetShowDetailsResponse> GetShowDetailsAsync(GetShowDetailsRequest request)
	{
		string requestUri = $"shows/showdetails/{request.Id.ToString(CultureInfo.InvariantCulture)}";
		return ClientAppRequest<GetShowDetailsResponse>(requestUri, HttpMethod.Get);
	}
}
