namespace WhatsOn.Web.Services.ShowApiService.Records;

public record Season
{
	public int Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string Overview { get; init; } = string.Empty;
	public int EpisodeCount { get; init; }
	public DateOnly? AirDate { get; init; }
	public string PosterPath { get; init; } = string.Empty;
	public int SeasonNumber { get; init; }
	public decimal VoteAverage { get; init; }
}
