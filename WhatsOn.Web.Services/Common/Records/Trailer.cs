namespace WhatsOn.Web.Services.Common.Records;

public record Trailer
{
	public string Name { get; init; } = string.Empty;
	public string Key { get; init; } = string.Empty;
	public string Site { get; init; } = string.Empty;
	public string Type { get; init; } = string.Empty;
	public bool Official { get; init; } = false;
	public string ThumbnailUrl { get; init; } = string.Empty;
	public string EmbedUrl { get; init; } = string.Empty;
}
