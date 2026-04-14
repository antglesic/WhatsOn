namespace WhatsOn.Web.Services.ShowApiService.Records;

public record Network
{
	public int Id { get; init; }
	public string LogoPath { get; init; } = string.Empty;
	public string Name { get; init; } = string.Empty;
	public string OriginCountry { get; init; } = string.Empty;
}
