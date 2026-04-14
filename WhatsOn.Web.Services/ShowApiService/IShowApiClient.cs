using WhatsOn.Web.Services.ShowApiService.Models;

namespace WhatsOn.Web.Services.ShowApiService;

public interface IShowApiClient
{
	Task<GetShowsResponse> GetShowsAsync(GetShowsRequest request, CancellationToken cancellationToken = default);
	Task<GetShowDetailsResponse> GetShowDetailsAsync(GetShowDetailsRequest request, CancellationToken cancellationToken = default);
}
