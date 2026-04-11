using WhatsOn.Domain;
using WhatsOn.Service.MovieService.Records;

namespace WhatsOn.Service.MovieService.Models;

public class GetMovieDetailsRequest : RequestBase
{
	public int Id { get; set; }
}

public class GetMovieDetailsResponse : ResponseBase<GetMovieDetailsRequest>
{
	public MovieDetailResponse MovieDetails { get; set; } = new();
}