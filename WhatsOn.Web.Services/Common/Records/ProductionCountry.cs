namespace WhatsOn.Web.Services.Common.Records;

public record ProductionCountry
{
	public string Iso31661 { get; init; } = string.Empty;
	public string Name { get; init; } = string.Empty;
}
