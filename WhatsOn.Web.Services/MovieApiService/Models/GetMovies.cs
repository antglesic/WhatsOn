using WhatsOn.Domain;
using WhatsOn.Web.Services.MovieApiService.Records;

namespace WhatsOn.Web.Services.MovieApiService.Models;

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
