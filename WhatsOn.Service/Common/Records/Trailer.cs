namespace WhatsOn.Service.Common.Records;

public record Trailer
{
	public string Name { get; init; } = string.Empty;
	public string Key { get; init; } = string.Empty;
	public string Site { get; init; } = string.Empty;
	public string Type { get; init; } = string.Empty;
	public bool Official { get; init; } = false;
	public string ThumbnailUrl
	{
		get
		{
			return $"https://img.youtube.com/vi/{Key}/hqdefault.jpg";
		}
	}
	public string EmbedUrl
	{
		get
		{
			return $"https://www.youtube.com/embed/{Key}?rel=0&showinfo=1";
		}
	}
}
