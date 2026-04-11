using WhatsOn.Domain;

namespace WhatsOn.Service.ShowService.Records;

public record ShowSearchResponse
{
	public int Page { get; init; }
	public int TotalPages { get; init; }
	public int TotalResults { get; init; }
	public IReadOnlyList<Show> Results { get; init; } = [];
	public PagedResult<Show> ToPagedResult() => new()
	{
		PageNumber = Page,
		PageSize = Results.Count,
		TotalPages = TotalPages,
		TotalItemCount = TotalResults,
		Data = Results
	};
}
