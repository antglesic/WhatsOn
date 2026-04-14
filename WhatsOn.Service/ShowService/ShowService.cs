using Microsoft.Extensions.Logging;
using System.Text.Json;
using WhatsOn.Service.ShowService.Models;
using WhatsOn.Service.ShowService.Records;

namespace WhatsOn.Service.ShowService;

public class ShowService(
	HttpClient httpClient,
	ILogger<ShowService> logger) : IShowService
{
	private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

	public async Task<GetShowsResponse> GetShows(GetShowsRequest request, CancellationToken cancellationToken)
	{
		GetShowsResponse response = new()
		{
			Request = request
		};

		try
		{
			var url = request.Query switch
			{
				{ Length: > 0 } query => $"search/tv?{BuildQueryString(new Dictionary<string, string>
				{
					["query"] = Uri.EscapeDataString(query),
					["page"] = request.PageNumber.ToString(),
					["include_adult"] = request.IncludeAdult.ToString()
				})}",
				_ => $"discover/tv" + (request.PageNumber.HasValue ? $"?page={request.PageNumber}" : string.Empty)
			};

			HttpResponseMessage responseMessage = await httpClient.GetAsync(url, cancellationToken);
			if (responseMessage is { IsSuccessStatusCode: true })
			{
				Stream responseStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
				if (responseStream is { Length: > 0 })
				{
					var shows = await JsonSerializer.DeserializeAsync<ShowSearchResponse>(responseStream, jsonSerializerOptions, cancellationToken);

					if (shows is { TotalResults: > 0 })
					{
						response.Shows = shows.ToPagedResult();
					}
					else
					{
						response.Message = "No tv shows were found!";
					}
				}
			}

			response.Success = true;
		}
		catch (HttpRequestException ex)
		{
			logger.LogError(ex, "HTTP error fetching tv shows for query={Query}", request.Query);
			response.Message = "Failed to reach the tv show database. Please try again later.";
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Unexpected error fetching tv shows for query={Query}", request.Query);
			response.Message = "An unexpected error occurred.";
		}

		return response;
	}

	public async Task<GetShowDetailsResponse> GetShowDetails(GetShowDetailsRequest request, CancellationToken cancellationToken)
	{
		GetShowDetailsResponse response = new()
		{
			Request = request
		};

		try
		{
			var url = $"tv/{request.Id}?append_to_response=videos";

			HttpResponseMessage responseMessage = await httpClient.GetAsync(url, cancellationToken);
			if (responseMessage is { IsSuccessStatusCode: true })
			{
				Stream responseStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
				if (responseStream is { Length: > 0 })
				{
					var show = await JsonSerializer.DeserializeAsync<ShowDetailResponse>(responseStream, jsonSerializerOptions, cancellationToken);

					if (show is not null)
					{
						response.ShowDetails = show;
					}
					else
					{
						response.Message = $"No movie was found for Id: {request.Id}!";
					}
				}
			}

			response.Success = true;
		}
		catch (HttpRequestException ex)
		{
			logger.LogError(ex, "HTTP error fetching tv show details for ShowId={Id}", request.Id);
			response.Message = "Failed to reach the tv show database. Please try again later.";
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Unexpected error fetching tv show details for ShowId={Id}", request.Id);
			response.Message = "An unexpected error occurred.";
		}

		return response;
	}

	private static string BuildQueryString(Dictionary<string, string> queryParams)
	{
		return string.Join("&", queryParams
			.Where(x => !string.IsNullOrEmpty(x.Value))
			.Select(x => $"{x.Key}={x.Value}"));
	}
}
