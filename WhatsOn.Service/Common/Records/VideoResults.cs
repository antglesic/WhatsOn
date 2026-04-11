namespace WhatsOn.Service.Common.Records;

public record VideoResults
{
	public string Iso6391 { get; init; } = string.Empty;
	public string Iso31661 { get; init; } = string.Empty;
	public string Name { get; init; } = string.Empty;
	public string Key { get; init; } = string.Empty;
	public string PublishedAt { get; init; } = string.Empty;
	public string Site { get; init; } = string.Empty;
	public int Size { get; init; }
	public string Type { get; init; } = string.Empty;
	public bool Official { get; init; } = false;
	public string Id { get; init; } = string.Empty;
}
