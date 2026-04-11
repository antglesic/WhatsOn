namespace WhatsOn.Service.MovieService.Records;

public record MovieCollection
{
	public int Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string? PosterUrl { get; init; } = string.Empty;
	public string? BackDropUrl { get; init; } = string.Empty;
}
