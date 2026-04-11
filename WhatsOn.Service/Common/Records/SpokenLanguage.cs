namespace WhatsOn.Service.Common.Records;

public record SpokenLanguage
{
	public string EnglishName { get; init; } = string.Empty;
	public string Iso6391 { get; init; } = string.Empty;
	public string Name { get; init; } = string.Empty;
}
