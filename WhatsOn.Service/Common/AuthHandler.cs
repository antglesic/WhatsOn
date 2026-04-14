using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using WhatsOn.Service.Common.Configuration;

namespace WhatsOn.Service.Common;

public sealed class TheMovieDbAuthHandler(IOptions<ExternalServicesSettings> options) : DelegatingHandler
{
	private readonly string _token = options.Value.TheMovieDbAccessToken;

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

		return base.SendAsync(request, cancellationToken);
	}
}

