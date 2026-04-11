using WhatsOn.Domain;

namespace WhatsOn.Service.MovieService.Records;

public record MovieSearchResponse
{
	public int Page { get; init; }
	public int TotalPages { get; init; }
	public int TotalResults { get; init; }
	public IReadOnlyList<Movie> Results { get; init; } = [];
	public PagedResult<Movie> ToPagedResult() => new()
	{
		PageNumber = Page,
		PageSize = Results.Count,
		TotalPages = TotalPages,
		TotalItemCount = TotalResults,
		Data = Results
	};
}