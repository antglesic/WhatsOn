using WhatsOn.Service.Common.Records;
using WhatsOn.Service.MovieService.Records;

namespace WhatsOn.Service.ShowService.Records;

public record ShowDetailResponse
{
	private const string ImageBaseUrl = "https://image.tmdb.org/t/p";
	public int Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string? Tagline { get; init; } = string.Empty;
	public string? Overview { get; init; } = string.Empty;
	public string? Status { get; init; } = string.Empty;
	public double VoteAverage { get; init; }
	public int VoteCount { get; init; }
	public double Popularity { get; init; }
	public string Homepage { get; init; } = string.Empty;
	public bool InProduction { get; init; } = false;
	public DateOnly? FirstAirDate { get; init; }
	public DateOnly? LastAirDate { get; init; }
	public EpisodeDetails? LastEpisode { get; init; }
	public EpisodeDetails? NextEpisodeToAir { get; init; }
	public int NumberOfEpisodes { get; init; }
	public int NumberOfSeasons { get; init; }
	public IReadOnlyList<string> Languages { get; init; } = [];
	public IReadOnlyList<string> OriginCountry { get; init; } = [];
	public IReadOnlyList<Network> Networks { get; init; } = [];
	public IReadOnlyList<int> EpisodeRunTime { get; init; } = [];
	public IReadOnlyList<Creator> CreatedBy { get; init; } = [];
	public IReadOnlyList<Genre> Genres { get; init; } = [];
	public IReadOnlyList<ProductionCountry> ProductionCountries { get; init; } = [];
	public IReadOnlyList<SpokenLanguage> SpokenLanguages { get; init; } = [];
	public IReadOnlyList<Season> Seasons { get; init; } = [];
	public Video? Videos { get; init; }
	public IReadOnlyList<ProductionCompany> ProductionCompanies { get; init; } = [];

	private string? _posterPath;
	private string? _backdropPath;
	private IReadOnlyList<Trailer>? _trailers;

	public string? PosterPath
	{
		get => _posterPath is null ? null : $"{ImageBaseUrl}/w500{_posterPath}";
		init => _posterPath = value;
	}
	public string? BackdropPath
	{
		get => _backdropPath is null ? null : $"{ImageBaseUrl}/w1280{_backdropPath}";
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
