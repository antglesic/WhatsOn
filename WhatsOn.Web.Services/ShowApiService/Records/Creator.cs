namespace WhatsOn.Web.Services.ShowApiService.Records;

public record Creator
{
	public int Id { get; init; }
	public string CreditId { get; init; } = string.Empty;
	public string Name { get; init; } = string.Empty;
	public string OriginalName { get; init; } = string.Empty;
	public int Gender { get; init; }
	public string ProfilePath { get; init; } = string.Empty;
}
