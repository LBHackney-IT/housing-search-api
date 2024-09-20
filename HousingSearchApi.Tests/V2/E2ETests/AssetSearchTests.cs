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
    private readonly HttpClient _httpClient;

    public GetAssetStoriesV2(MockWebApplicationFactory<Startup> factory)
    {
        _httpClient = factory.CreateClient();
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
    [InlineData("Gge 7 Toby Crossroad", "71312edd-10c3-41cf-b298-b97cce0c0123")]
    [InlineData("6 Philip Alley", "5a6067de-458c-40cb-93a3-d16fe39da12a")]
    [InlineData("Room 3 2 Kevin Inlet", "b5e85565-6241-4dd2-8f1f-50d626b129f2")]
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

    [Theory]
    [InlineData("NW1 3LF")]
    [InlineData("L22 9BQ")]
    [InlineData("PA3R 7JR")]
    public async Task ReturnsRelevantResultFirstByPostcode(string searchTextPostcode)
    {
        // Arrange
        var request = CreateSearchRequest(searchTextPostcode);

        // Act
        var response = await _httpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var root = GetResponseRootElement(response);
        root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
        var firstResult = root.GetProperty("results").GetProperty("assets")[0];
        firstResult.GetProperty("assetAddress").GetProperty("postCode").GetString().Should().Be(searchTextPostcode);
    }

    [Theory]
    [InlineData("Gge 7 Toby CA18", "71312edd-10c3-41cf-b298-b97cce0c0123")]
    [InlineData("6 Philip Alley BH62 8DR", "5a6067de-458c-40cb-93a3-d16fe39da12a")]
    [InlineData("Room 3 2 Kevin BR49 6LU", "b5e85565-6241-4dd2-8f1f-50d626b129f2")]
    public async Task ReturnsRelevantResultFirstByAddressAndPostcode(string searchText, string expectedReturnedId)
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

    [Theory]
    [InlineData("154455647", "1d3f9e8e-d380-441f-97bb-2d280aecb80f")]
    public async Task ReturnsRelevantResultFirstByPaymentRef(string searchText, string expectedReturnedId)
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

    [Theory]
    [InlineData("171623604", "9309e7d5-3d4b-4091-9fc5-6bd7c8e1a436")]
    public async Task ReturnsRelevantResultFirstByAssetId(string searchText, string expectedReturnedId)
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

