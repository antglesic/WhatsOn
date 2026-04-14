namespace WhatsOn.Web.Services.Common.Records;

public record Video
{
	public IReadOnlyList<VideoResults> Results { get; init; } = [];
}
