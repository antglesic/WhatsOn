using WhatsOn.Web.Services.Common.Records;
using WhatsOn.Web.Services.MovieApiService.Records;

namespace WhatsOn.Web.Services.ShowApiService.Records;

public record ShowDetails
{
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
	public DateOnly FirstAirDate { get; init; }
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
	public IReadOnlyList<ProductionCompany> ProductionCompanies { get; init; } = [];
	public string? PosterPath { get; init; } = string.Empty;
	public string? BackdropPath { get; init; } = string.Empty;
	public IReadOnlyList<Trailer> Trailers { get; init; } = [];
}
