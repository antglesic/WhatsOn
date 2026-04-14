using WhatsOn.Service.Common.Records;

namespace WhatsOn.Service.MovieService.Records;

public record MovieDetailResponse
{
	private const string ImageBaseUrl = "https://image.tmdb.org/t/p";
	public int Id { get; init; }
	public string Title { get; init; } = string.Empty;
	public string OriginalTitle { get; init; } = string.Empty;
	public string Tagline { get; init; } = string.Empty;
	public string Overview { get; init; } = string.Empty;
	public string Status { get; init; } = string.Empty;
	public string ReleaseDate { get; init; } = string.Empty;
	public string Homepage { get; init; } = string.Empty;
	public string ImdbId { get; init; } = string.Empty;
	public string Language { get; init; } = string.Empty;
	public int? Runtime { get; init; }
	public long Budget { get; init; }
	public long Revenue { get; init; }
	public double VoteAverage { get; init; }
	public int VoteCount { get; init; }
	public double Popularity { get; init; }
	public IReadOnlyList<Genre> Genres { get; init; } = [];
	public IReadOnlyList<ProductionCountry> ProductionCountries { get; init; } = [];
	public IReadOnlyList<SpokenLanguage> SpokenLanguages { get; init; } = [];
	public Video Videos { get; init; }
	public IReadOnlyList<ProductionCompany> ProductionCompanies { get; init; } = [];
	public MovieCollection MovieCollection { get; init; }

	private string _posterPath;
	private string _backdropPath;
	private IReadOnlyList<Trailer> _trailers;

	public string PosterPath
	{
		get => _posterPath is null ? string.Empty : $"{ImageBaseUrl}/w500{_posterPath}";
		init => _posterPath = value;
	}
	public string BackdropPath
	{
		get => _backdropPath is null ? string.Empty : $"{ImageBaseUrl}/w1280{_backdropPath}";
		init => _backdropPath = value;
	}

	public IReadOnlyList<Trailer> Trailers
	{
		get => Videos?.Results is { Count: > 0 }
			? [.. Videos.Results
			.Where(t => t.Site == "YouTube" && t.Type is "Trailer" or "Teaser")
			.Select(t => new Trailer
			{
				Key = t.Key,
				Name = t.Name,
				Site = t.Site,
				Type = t.Type,
				Official = t.Official
			})]
			: [];
		init => _trailers = value;
	}
}