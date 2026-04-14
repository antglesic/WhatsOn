using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WhatsOn.Domain;

namespace WhatsOn.Web.Services;

public abstract class WhatsOnApiClientBase(HttpClient httpClient)
{
	private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
	{
		PropertyNameCaseInsensitive = true
	};

	protected async Task<TResponse> SendGetAsync<TResponse, TRequest>(
		string requestUri,
		TRequest request,
		CancellationToken cancellationToken)
		where TResponse : ResponseBase<TRequest>, new()
		where TRequest : RequestBase
	{
		try
		{
			using HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);

			if (response.IsSuccessStatusCode)
			{
				TResponse? result = await response.Content.ReadFromJsonAsync<TResponse>(JsonSerializerOptions, cancellationToken);
				if (result is not null)
				{
					result.Request ??= request;
					return result;
				}

				return CreateFailureResponse<TResponse, TRequest>(request, "The API returned an empty response.");
			}

			if (response.StatusCode == HttpStatusCode.BadRequest)
			{
				ApiErrorResponse? error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(JsonSerializerOptions, cancellationToken);
				return CreateFailureResponse<TResponse, TRequest>(
					request,
					string.IsNullOrWhiteSpace(error?.Message) ? "The request could not be processed." : error.Message);
			}

			return CreateFailureResponse<TResponse, TRequest>(
				request,
				$"The API returned an unexpected status code: {(int)response.StatusCode}.");
		}
		catch (HttpRequestException)
		{
			return CreateFailureResponse<TResponse, TRequest>(request, "Failed to reach the WhatsOn API. Please try again later.");
		}
		catch (JsonException)
		{
			return CreateFailureResponse<TResponse, TRequest>(request, "The API returned data in an unexpected format.");
		}
	}

	protected static string BuildRelativeUri(string path, IReadOnlyDictionary<string, string?> queryParameters)
	{
		string queryString = BuildQueryString(queryParameters);
		return string.IsNullOrWhiteSpace(queryString) ? path : $"{path}?{queryString}";
	}

	private static string BuildQueryString(IReadOnlyDictionary<string, string?> queryParameters)
	{
		return string.Join(
			"&",
			queryParameters
				.Where(entry => !string.IsNullOrWhiteSpace(entry.Value))
				.Select(entry => $"{entry.Key}={Uri.EscapeDataString(entry.Value!)}"));
	}

	private static TResponse CreateFailureResponse<TResponse, TRequest>(TRequest request, string message)
		where TResponse : ResponseBase<TRequest>, new()
		where TRequest : RequestBase
	{
		return new TResponse
		{
			Request = request,
			Message = message
		};
	}
}
