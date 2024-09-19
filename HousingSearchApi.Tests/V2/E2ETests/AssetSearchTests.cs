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
    [InlineData("148 Kingsland", "83458161-e28e-1d05-72d9-6af5699d1b59")]
    [InlineData("Flat 421 359 Kingsland", "c300313f-d4ca-6e0e-48c2-7ad2961436ac")]
    [InlineData("Gge 102 Kingsland", "aaccd37d-f0a0-80da-acab-0ea00d943ca5")]
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

