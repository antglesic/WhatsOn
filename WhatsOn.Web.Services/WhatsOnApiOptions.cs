namespace WhatsOn.Web.Services;

public sealed class WhatsOnApiOptions
{
	public const string SectionName = "WhatsOnApi";

	public string BaseUrl { get; init; } = string.Empty;
}
