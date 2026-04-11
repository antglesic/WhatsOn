using WhatsOn.Domain;
using WhatsOn.Service.MovieService.Records;

namespace WhatsOn.Service.MovieService.Models;

public class GetMoviesRequest : RequestBase
{
	public string Query { get; set; } = string.Empty;
	public int? PageNumber { get; set; }
	public bool? IncludeAdult { get; set; } = false;
}

public class GetMoviesResponse : ResponseBase<GetMoviesRequest>
{
	public PagedResult<Movie> Movies { get; set; } = new();
}
