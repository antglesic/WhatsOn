using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using WhatsOn.Domain;
using WhatsOn.Service.ShowService.Models;
using WhatsOn.Service.ShowService.Records;

namespace WhatsOn.Web.Services.Tests;

public class ShowApiClientTests
{
	[Fact]
	public async Task GetShowsAsync_BuildsExpectedQueryString()
	{
		StubHttpMessageHandler handler = new((_, _) => Task.FromResult(CreateJsonResponse(new GetShowsResponse
		{
			Success = true,
			Shows = new PagedResult<Show>
			{
				PageNumber = 3,
				TotalPages = 10,
				Data = [new Show { Id = 11, Name = "The Office" }]
			}
		})));

		ShowApiClient client = CreateClient(handler);

		GetShowsResponse response = await client.GetShowsAsync(new GetShowsRequest
		{
			Query = "the office",
			PageNumber = 3,
			IncludeAdult = false
		});

		Assert.Equal("/shows/getshows?query=the%20office&pageNumber=3&includeAdult=False", handler.LastRequest?.RequestUri?.PathAndQuery);
		Assert.True(response.Success);
		Assert.Single(response.Shows.Data);
	}

	[Fact]
	public async Task GetShowDetailsAsync_UsesShowDetailsRoute()
	{
		StubHttpMessageHandler handler = new((_, _) => Task.FromResult(CreateJsonResponse(new GetShowDetailsResponse
		{
			Success = true,
			ShowDetails = new ShowDetailResponse
			{
				Id = 42,
				Name = "Severance"
			}
		})));

		ShowApiClient client = CreateClient(handler);

		GetShowDetailsResponse response = await client.GetShowDetailsAsync(new GetShowDetailsRequest { Id = 42 });

		Assert.Equal("/shows/showdetails/42", handler.LastRequest?.RequestUri?.PathAndQuery);
		Assert.True(response.Success);
		Assert.Equal(42, response.ShowDetails?.Id);
	}

	[Fact]
	public async Task GetShowsAsync_ReturnsFallbackMessageOnTransportFailure()
	{
		StubHttpMessageHandler handler = new((_, _) => throw new HttpRequestException("boom"));
		ShowApiClient client = CreateClient(handler);

		GetShowsResponse response = await client.GetShowsAsync(new GetShowsRequest { Query = "severance" });

		Assert.False(response.Success);
		Assert.Equal("Failed to reach the WhatsOn API. Please try again later.", response.Message);
		Assert.NotNull(response.Request);
	}

	private static ShowApiClient CreateClient(HttpMessageHandler handler)
	{
		return new ShowApiClient(new HttpClient(handler)
		{
			BaseAddress = new Uri("https://api.whats-on.test/")
		});
	}

	private static HttpResponseMessage CreateJsonResponse<T>(T payload, HttpStatusCode statusCode = HttpStatusCode.OK)
	{
		return new HttpResponseMessage(statusCode)
		{
			Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json)
		};
	}
}
