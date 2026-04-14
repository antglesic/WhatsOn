using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;

namespace WhatsOn.Web.Services.Common.ApiClientBase;

/// <summary>
/// Provides shared HTTP request, authentication, query-string, and JSON serialization behavior
/// for finance-service integrations that call internal external APIs.
/// </summary>
/// <remarks>
/// Derived services are expected to receive a configured <see cref="HttpClient"/> from dependency injection,
/// usually with <see cref="HttpClient.BaseAddress"/> already set. Callers typically use relative endpoint paths
/// and handle a possible <see langword="default"/> result when the remote API responds unsuccessfully, returns an
/// empty body, or returns JSON that cannot be deserialized into the requested response type.
/// </remarks>
public abstract class WhatsOnApiClientBase
{

	protected readonly HttpClient _client;
	protected readonly ILogger _logger;
	protected readonly JsonSerializerOptions _jsonSerializerOptions;
	private static readonly HttpMethod[] MethodsWithBody =
	[
		HttpMethod.Post,
		HttpMethod.Put,
		HttpMethod.Patch
	];

	/// <summary>
	/// Initializes the shared external-service infrastructure.
	/// </summary>
	/// <param name="client">
	/// The configured <see cref="HttpClient"/> used to send requests. The client should normally be created by DI
	/// and already contain the expected <see cref="HttpClient.BaseAddress"/> for the target API.
	/// </param>
	/// <param name="logger">The logger used for transport, status-code, and serialization diagnostics.</param>
	/// <param name="jsonSerializerOptions">
	/// Optional serializer options used for request-body serialization and response deserialization.
	/// When omitted, a default instance with case-insensitive property matching is used.
	/// </param>
	protected WhatsOnApiClientBase(HttpClient client, ILogger logger, JsonSerializerOptions jsonSerializerOptions = null)
	{
		_client = client;
		_jsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		_logger = logger;
	}

	/// <summary>
	/// Sends an authenticated HTTP request and deserializes the response body into <typeparamref name="TResponse"/>.
	/// </summary>
	/// <typeparam name="TResponse">The expected response-body type.</typeparam>
	/// <param name="url">A relative or absolute request URL.</param>
	/// <param name="method">The HTTP method to send.</param>
	/// <param name="parameters">Optional query-string parameters appended to the request URL.</param>
	/// <param name="body">Optional request body. Bodies are intended for <c>POST</c>, <c>PUT</c>, and <c>PATCH</c> requests.</param>
	/// <returns>
	/// The deserialized response body when the request succeeds and the body can be parsed;
	/// otherwise <see langword="default"/>.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the request body cannot be serialized.
	/// </exception>
	/// <exception cref="HttpRequestException">
	/// Thrown when the HTTP request cannot be sent.
	/// </exception>
	/// <remarks>
	/// logs unsuccessful status codes, and disposes the <see cref="HttpResponseMessage"/> before returning.
	/// <para>
	/// <see cref="HttpClient.SendAsync(HttpRequestMessage)"/> is intentionally isolated in its own narrow <c>try/catch</c>
	/// so transport failures are wrapped as <see cref="HttpRequestException"/> without also catching response-reading
	/// or deserialization failures. Once sending succeeds, the response is disposed in a separate <c>using</c> block
	/// after the body has been processed.
	/// </para>
	/// <para>
	/// If the API returns a non-success status code, an empty body, or JSON that cannot be deserialized,
	/// the method logs the condition and returns <see langword="default"/>.
	/// </para>
	/// </remarks>
	protected async Task<TResponse> ClientAppRequest<TResponse>(string url, HttpMethod method, NameValueCollection parameters = null, object body = null)
	{
		using HttpResponseMessage response = await SendClientAppRequest(url, method, parameters, body);
		var responseContent = await response.Content.ReadAsStringAsync();
		string requestUri = response.RequestMessage is null
			? url
			: GetRequestUriForLogging(response.RequestMessage, url);

		if (!response.IsSuccessStatusCode)
		{
			_logger.LogWarning(
				"API returned {StatusCode} for {Method} {Url}. Response: {Response}",
				response.StatusCode,
				method,
				requestUri,
				responseContent);
			return default;
		}

		if (string.IsNullOrWhiteSpace(responseContent))
		{
			_logger.LogWarning("Empty response body for {Method} {Url}", method, requestUri);
			return default;
		}

		try
		{
			return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);
		}
		catch (JsonException ex)
		{
			_logger.LogError(ex,
				"Failed to deserialize response for {Method} {Url}. Response: {Response}",
				method,
				requestUri,
				responseContent);
			return default;
		}
	}

	/// <summary>
	/// Sends an authenticated HTTP request and returns the raw response for callers that must handle
	/// status codes or response processing themselves.
	/// </summary>
	/// <param name="url">A relative or absolute request URL.</param>
	/// <param name="method">The HTTP method to send.</param>
	/// <param name="parameters">Optional query-string parameters appended to the request URL.</param>
	/// <param name="body">Optional request body. Bodies are intended for <c>POST</c>, <c>PUT</c>, and <c>PATCH</c> requests.</param>
	/// <returns>The raw <see cref="HttpResponseMessage"/>. The caller is responsible for disposing it.</returns>
	protected async Task<HttpResponseMessage> SendClientAppRequest(string url, HttpMethod method, NameValueCollection parameters = null, object body = null)
	{
		using var request = new HttpRequestMessage(method, url);

		if (body is not null)
		{
			if (!MethodsWithBody.Contains(method))
			{
				_logger.LogWarning("{Method} request to {Url} should not contain a body", method, url);
			}

			try
			{
				var json = JsonSerializer.Serialize(body, _jsonSerializerOptions);
				request.Content = new StringContent(json, Encoding.UTF8, "application/json");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to serialize request body for {Method} {Url}", method, url);
				throw new InvalidOperationException($"Failed to serialize request body for {method} {url}.", ex);
			}
		}

		if (parameters is not null)
		{
			AddParameters(request, parameters);
		}

		try
		{
			return await _client.SendAsync(request);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception occurred while sending {Method} request to {Url}", method, GetRequestUriForLogging(request, url));
			throw new HttpRequestException($"Error sending {method} request to {url}", ex);
		}
	}

	/// <summary>
	/// Appends query-string parameters to the current request URI.
	/// </summary>
	/// <param name="request">The request whose URI will be updated.</param>
	/// <param name="parameters">The parameters to append. Multiple values for the same key are preserved.</param>
	protected static void AddParameters(HttpRequestMessage request, NameValueCollection parameters)
	{
		if (request.RequestUri == null || parameters == null || parameters.Count == 0)
			return;

		// Uzimamo trenutni relatvni ULR
		string uri = request.RequestUri.ToString();

		foreach (string key in parameters.AllKeys)
		{
			var values = parameters.GetValues(key);
			if (values == null) continue;

			foreach (string value in values)
			{
				uri = QueryHelpers.AddQueryString(uri, key, value);
			}
		}

		//vracamo ga u modificirani URI u URL
		request.RequestUri = new Uri(uri, UriKind.RelativeOrAbsolute);
	}

	private static string GetRequestUriForLogging(HttpRequestMessage request, string fallbackUrl)
	{
		if (request.RequestUri is null)
			return fallbackUrl;

		return request.RequestUri.IsAbsoluteUri ? request.RequestUri.PathAndQuery : request.RequestUri.OriginalString;
	}

	protected static string BuildRelativeUri(string path, IReadOnlyDictionary<string, string> queryParameters)
	{
		string queryString = BuildQueryString(queryParameters);
		return string.IsNullOrWhiteSpace(queryString) ? path : $"{path}?{queryString}";
	}

	private static string BuildQueryString(IReadOnlyDictionary<string, string> queryParameters)
	{
		return string.Join(
			"&",
			queryParameters
				.Where(entry => !string.IsNullOrWhiteSpace(entry.Value))
				.Select(entry => $"{entry.Key}={Uri.EscapeDataString(entry.Value!)}"));
	}
}
