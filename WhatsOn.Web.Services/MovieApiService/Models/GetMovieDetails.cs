using WhatsOn.Domain;
using WhatsOn.Web.Services.MovieApiService.Records;

namespace WhatsOn.Web.Services.MovieApiService.Models;

public class GetMovieDetailsRequest : RequestBase
{
	public int Id { get; set; }
}

public class GetMovieDetailsResponse : ResponseBase<GetMovieDetailsRequest>
{
	public MovieDetails MovieDetails { get; set; } = new();
}
