using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    // Return a random asset from the assets.json file
    private JsonElement RandomAsset()
    {
        using StreamReader r = new StreamReader("V2/E2ETests/Fixtures/assets.json");
        string json = r.ReadToEnd();
        List<string> splitLines = new List<string>(json.Split("\n"))
            .Where(line => !line.Contains("index")
            ).ToList();

        Func<string, JsonDocument> TryParse = str_json =>
        {
            try
            {
                return JsonDocument.Parse(str_json);
            }
            catch (JsonException)
            {
                return null;
            }
        };

        var assets = splitLines.Select(line => TryParse(line)?.RootElement).Where(x => x != null);
        var jsonElements = assets as JsonElement?[] ?? assets.ToArray();
        var asset = jsonElements.ElementAt(new Random().Next(jsonElements.Count()));
        return (JsonElement) asset;
    }

    public GetAssetStoriesV2(MockWebApplicationFactory<Startup> factory)
    {
        _httpClient = factory.CreateClient();
    }

    private HttpRequestMessage CreateSearchRequest(string searchText) =>
        new HttpRequestMessage(
            HttpMethod.Get,
            $"http://localhost:3000/api/v2/search/assets/?searchText={searchText}"
            );


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

    [Fact]
    public async Task ReturnsRelevantResultFirstByAddress()
    {
        foreach (var _ in Enumerable.Range(0, 10))
        {
            // Arrange
            var randomAsset = RandomAsset();
            var expectedReturnedId = randomAsset.GetProperty("id").GetString();
            var request = CreateSearchRequest(randomAsset.GetProperty("assetAddress").GetProperty("addressLine1").GetString());

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

    [Fact]
    public async Task ReturnsRelevantResultFirstByPostcode()
    {
        foreach (var _ in Enumerable.Range(0, 10))
        {
            // Arrange
            var randomAsset = RandomAsset();
            var searchTextPostcode = randomAsset.GetProperty("assetAddress").GetProperty("postCode").GetString();
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
    }

    [Fact]
    public async Task ReturnsRelevantResultFirstByAddressAndPostcode()
    {
        foreach (var _ in Enumerable.Range(0, 10))
        {
            // Arrange
            var randomAsset = RandomAsset();
            var expectedReturnedId = randomAsset.GetProperty("id").GetString();
            var searchText = randomAsset.GetProperty("assetAddress").GetProperty("addressLine1").GetString() + " " + randomAsset.GetProperty("assetAddress").GetProperty("postCode").GetString();
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

    [Fact]
    public async Task ReturnsRelevantResultFirstByPaymentRef()
    {
        // Arrange
        var asset = RandomAsset();
        var expectedReturnedId = asset.GetProperty("id").GetString();
        var searchText = asset.GetProperty("tenure").GetProperty("paymentReference").GetString();
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

    [Fact]
    public async Task ReturnsRelevantResultFirstByAssetId()
    {
        // Arrange
        var asset = RandomAsset();
        var expectedReturnedId = asset.GetProperty("id").GetString();
        var searchText = asset.GetProperty("assetId").GetString();
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

