using WhatsOn.Domain;
using WhatsOn.Service.ShowService.Records;

namespace WhatsOn.Service.ShowService.Models;

public class GetShowsRequest : RequestBase
{
	public string Query { get; set; } = string.Empty;
	public int? PageNumber { get; set; }
	public bool? IncludeAdult { get; set; } = false;
}

public class GetShowsResponse : ResponseBase<GetShowsRequest>
{
	public PagedResult<Show> Shows { get; set; } = new();
}