using WhatsOn.Domain;
using WhatsOn.Web.Services.ShowApiService.Records;

namespace WhatsOn.Web.Services.ShowApiService.Models;

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