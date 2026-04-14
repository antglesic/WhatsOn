using WhatsOn.Domain;
using WhatsOn.Web.Services.ShowApiService.Records;

namespace WhatsOn.Web.Services.ShowApiService.Models;

public class GetShowDetailsRequest : RequestBase
{
	public int Id { get; set; }
}

public class GetShowDetailsResponse : ResponseBase<GetShowDetailsRequest>
{
	public ShowDetails ShowDetails { get; set; } = new();
}
