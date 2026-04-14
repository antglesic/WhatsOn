namespace WhatsOn.Web.Services.MovieApiService.Records;

public record Movie
{
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
	public string PosterPath { get; init; } = string.Empty;
}
