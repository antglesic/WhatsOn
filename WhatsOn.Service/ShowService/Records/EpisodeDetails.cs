namespace WhatsOn.Service.ShowService.Records;

public record EpisodeDetails
{
	public int Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string Overview { get; init; } = string.Empty;
	public double VoteAverage { get; init; }
	public int VoteCount { get; init; }
	public DateOnly? AirDate { get; init; }
	public int EpisodeNumber { get; init; }
	public string EpisodeType { get; init; } = string.Empty;
	public string ProductionCode { get; init; } = string.Empty;
	public int RunTime { get; init; }
	public int SeasonNumber { get; init; }
	public int ShowId { get; init; }
	public string StillPath { get; init; } = string.Empty;
}
