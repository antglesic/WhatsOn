using WhatsOn.Service.ShowService.Models;

namespace WhatsOn.Service.ShowService;

public interface IShowService
{
	Task<GetShowsResponse> GetShows(GetShowsRequest request, CancellationToken cancellationToken);
	Task<GetShowDetailsResponse> GetShowDetails(GetShowDetailsRequest request, CancellationToken cancellationToken);
}
