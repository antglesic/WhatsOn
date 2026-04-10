namespace WhatsOn.Service.MovieService.Records;

public record Movie
{
	public int Id { get; init; }
	public string Title { get; init; } = string.Empty;
}

