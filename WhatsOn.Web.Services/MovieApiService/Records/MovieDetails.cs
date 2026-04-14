using WhatsOn.Web.Services.Common.Records;

namespace WhatsOn.Web.Services.MovieApiService.Records;

public record MovieDetails
{
	public int Id { get; init; }
	public string Title { get; init; } = string.Empty;
	public string? OriginalTitle { get; init; } = string.Empty;
	public string? Tagline { get; init; } = string.Empty;
	public string? Overview { get; init; } = string.Empty;
	public string? Status { get; init; } = string.Empty;
	public string? ReleaseDate { get; init; } = string.Empty;
	public string? Homepage { get; init; } = string.Empty;
	public string? ImdbId { get; init; } = string.Empty;
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
	public IReadOnlyList<ProductionCompany> ProductionCompanies { get; init; } = [];
	public MovieCollection? MovieCollection { get; init; }
	public string? PosterPath { get; init; } = string.Empty;
	public string? BackdropPath { get; init; } = string.Empty;
	public IReadOnlyList<Trailer> Trailers { get; init; } = [];
}