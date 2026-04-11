namespace WhatsOn.Service.Common.Records;

public record Video
{
	public IReadOnlyList<VideoResults> Results { get; init; } = [];
}
