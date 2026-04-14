namespace WhatsOn.Service.MovieService.Records;

public record Movie
{
	private const string ImageBaseUrl = "https://image.tmdb.org/t/p";

	public int Id { get; init; }
	public string Title { get; init; } = string.Empty;
	public string Overview { get; init; } = string.Empty;
	public string ReleaseDate { get; init; } = string.Empty;
	public double VoteAverage { get; init; }
	public int VoteCount { get; init; }
	public double Popularity { get; init; }
	public string OriginalLanguage { get; init; } = string.Empty;
	public string OriginalTitle { get; init; }
	public List<int> GenreIds { get; init; } = [];
	public bool Adult { get; init; } = false;
	public bool Video { get; init; } = false;

	private string _posterPath;

	public string PosterPath
	{
		get => _posterPath is null ? null : $"{ImageBaseUrl}/w500{_posterPath}";
		init => _posterPath = value;
	}
}