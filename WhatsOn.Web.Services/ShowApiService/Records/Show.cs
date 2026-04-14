namespace WhatsOn.Web.Services.ShowApiService.Records;

public record Show
{
	public int Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string Overview { get; init; } = string.Empty;
	public string ReleaseDate { get; init; } = string.Empty;
	public double VoteAverage { get; init; }
	public int VoteCount { get; init; }
	public double Popularity { get; init; }
	public string OriginalLanguage { get; init; } = string.Empty;
	public string OriginalName { get; init; }
	public List<int> GenreIds { get; init; } = [];
	public List<string> OriginCountry { get; init; } = [];
	public bool Adult { get; init; } = false;
	public bool Video { get; init; } = false;
	public DateOnly? FirstAirDate { get; init; }
	public string PosterPath { get; init; } = string.Empty;
}
