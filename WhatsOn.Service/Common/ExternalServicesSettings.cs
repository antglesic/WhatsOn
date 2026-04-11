using System.ComponentModel.DataAnnotations;

namespace WhatsOn.Service.Common;

public sealed class ExternalServicesSettings
{
	public const string SectionName = "ExternalServicesSettings";

	[Required]
	[Url]
	public string TheMovieDbDocumentationApiBaseUrl { get; init; } = string.Empty;

	[Required]
	public string TheMovieDbAccessToken { get; init; } = string.Empty;
}
