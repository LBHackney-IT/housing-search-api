using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using HousingSearchApi.Tests.V2.E2ETests.Fixtures;

namespace HousingSearchApi.Tests.V2.E2ETests;

public class AssetSearchTests : BaseSearchTests
{
    private readonly HttpClient _httpClient;

    public AssetSearchTests(CombinedFixture combinedFixture) : base(combinedFixture, indexName: "assets")
    {
        _httpClient = combinedFixture.Factory.CreateClient();
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
        root.GetProperty("results").GetProperty("assets").GetArrayLength().Should().Be(0);
    }

    [Fact]
    public async Task ReturnsRelevantResultFirstByAddress()
    {
        const int attempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(attempts, async () =>
        {
            // Arrange
            var randomAsset = RandomItem();
            var expectedReturnedId = randomAsset.GetProperty("id").GetString();
            var query = randomAsset.GetProperty("assetAddress").GetProperty("addressLine1").GetString();
            var request = CreateSearchRequest(query);

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("assets")[0];
            firstResult.GetProperty("id").GetString().Should().Be(expectedReturnedId);
        });
        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task ReturnsRelevantResultFirstByPostcode()
    {
        const int maxAttempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(maxAttempts, async () =>
        {
            // Arrange
            var randomAsset = RandomItem();
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
        });
        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task ReturnsRelevantResultFirstByAddressAndPostcode()
    {
        const int maxAttempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(maxAttempts, async () =>
        {
            // Arrange
            var randomAsset = RandomItem();
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
        });
        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task ReturnsRelevantResultFirstByAddressPrefix()
    {
        const int maxAttempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(maxAttempts, async () =>
        {
            // Arrange
            var randomAsset = RandomItem();
            var expectedReturnedId = randomAsset.GetProperty("id").GetString();
            var searchText = randomAsset.GetProperty("assetAddress").GetProperty("addressLine1").GetString()?[..12];
            var request = CreateSearchRequest(searchText);

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("assets")[0];
            firstResult.GetProperty("id").GetString().Should().Be(expectedReturnedId);
        });
        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task ReturnsRelevantResultFirstByAddressPartial()
    {
        const int maxAttempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(maxAttempts, async () =>
        {
            // Arrange
            var randomAsset = RandomItem();
            var randomAddress = randomAsset.GetProperty("assetAddress").GetProperty("addressLine1").GetString();
            var searchTerms = randomAddress.Split(' ').ToList();
            // remove one search term
            searchTerms.RemoveAt(new Random().Next(searchTerms.Count));
            var searchText = string.Join(" ", searchTerms);
            var request = CreateSearchRequest(searchText);

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("assets")[0];
            searchTerms.ForEach(term => 
                firstResult.GetProperty("assetAddress").GetProperty("addressLine1").GetString().Should().Contain(term)
            );
        });
        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task ReturnsRelevantResultFirstByPaymentRef()
    {
        // Arrange
        var asset = RandomItem();
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
        var asset = RandomItem();
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

