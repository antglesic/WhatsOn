using WhatsOn.Domain;
using WhatsOn.Service.MovieService.Records;

namespace WhatsOn.Service.MovieService.Models;

public class GetMoviesRequest : RequestBase
{
	public string Query { get; init; } = string.Empty;
}

public class GetMoviesResponse : ResponseBase<GetMoviesRequest>
{
	public PagedResult<Movie> Movies { get; init; } = new();
}
