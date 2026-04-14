using WhatsOn.Web.Services.ShowApiService.Models;

namespace WhatsOn.Web.Services.ShowApiService;

public interface IShowApiClient
{
	Task<GetShowsResponse> GetShowsAsync(GetShowsRequest request);
	Task<GetShowDetailsResponse> GetShowDetailsAsync(GetShowDetailsRequest request);
}
