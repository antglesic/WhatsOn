namespace WhatsOn.Service.MovieService.Records;

public record Genre
{
	public int Id { get; init; }
	public string Name { get; init; } = string.Empty;
}
