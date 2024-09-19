using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using System.Text.Json;

namespace HousingSearchApi.Tests.V2.E2ETests;

public class GetAssetStoriesV2 : IClassFixture<MockWebApplicationFactory<Startup>>
{
    // Note: see assets.json for the data that is being searched
    private readonly MockWebApplicationFactory<Startup> _factory;
    private readonly HttpClient _httpClient;

    public GetAssetStoriesV2(MockWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
        var elasticClient = factory.ElasticSearchClient;
    }

    private HttpRequestMessage CreateSearchRequest(string searchText)
    {
        return new HttpRequestMessage(
            HttpMethod.Get,
            $"http://localhost:3000/api/v2/search/assets/?searchText={searchText}"
            );
    }

    private JsonElement GetResponseRootElement(HttpResponseMessage response)
    {
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var doc = JsonDocument.Parse(responseBody);
        return doc.RootElement;
    }

    [Fact]
    public async Task ReturnsNoResultsWhenNoMatches()
    {
        // Arrange
        var request = CreateSearchRequest("XXXXXXXX");

        // Act
        var response = await _httpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var root = GetResponseRootElement(response);
        root.GetProperty("total").GetInt32().Should().Be(0);
    }

    [Theory]
    [InlineData("Gge 3b Price", "1de2964d-59a8-4e0e-b2e2-c3722b5f2a6b")]
    [InlineData("Flat 6 Charles", "660c8a83-9b4c-4944-890e-51a8e7ab1d1b")]
    [InlineData("10 Norris views", "e1e65cb3-b6f8-4536-9702-13712c8d5d12")]
    public async Task ReturnsRelevantResultFirstByAddress(string searchText, string expectedReturnedId)
    {
        // Arrange
        var request = CreateSearchRequest(searchText);

        // Act
        var response = await _httpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var root = GetResponseRootElement(response);

        root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);

        var firstResult = root.GetProperty("results").GetProperty("assets")[0];
        firstResult.GetProperty("id").GetString().Should().Be(expectedReturnedId);
    }
}

