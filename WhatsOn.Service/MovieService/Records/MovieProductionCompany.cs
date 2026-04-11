namespace WhatsOn.Service.MovieService.Records;

public record MovieProductionCompany
{
	public int Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string? LogoUrl { get; init; } = string.Empty;
	public string? OriginCountry { get; init; } = string.Empty;
}
