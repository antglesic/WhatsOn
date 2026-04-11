namespace WhatsOn.Service.MovieService.Records;

public record Video
{
	public IReadOnlyList<VideoResults> Results { get; init; } = [];
}
