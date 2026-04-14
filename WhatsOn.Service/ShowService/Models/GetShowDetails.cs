using WhatsOn.Domain;
using WhatsOn.Service.ShowService.Records;

namespace WhatsOn.Service.ShowService.Models;

public class GetShowDetailsRequest : RequestBase
{
	public int Id { get; set; }
}

public class GetShowDetailsResponse : ResponseBase<GetShowDetailsRequest>
{
	public ShowDetailResponse ShowDetails { get; set; } = new();
}
