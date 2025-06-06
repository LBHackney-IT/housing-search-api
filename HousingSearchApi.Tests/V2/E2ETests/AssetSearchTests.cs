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

    public AssetSearchTests(CombinedFixture combinedFixture) : base(combinedFixture, indexName: "assets") { }

    #region General

    [Fact]
    public async Task SearchNoMatch()
    {
        // Arrange
        var request = CreateSearchRequest("XXXXXXXX");

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var root = GetResponseRootElement(response);
        root.GetProperty("total").GetInt32().Should().Be(0);
        root.GetProperty("results").GetProperty("assets").GetArrayLength().Should().Be(0);
    }

    #endregion

    # region Address

    [Fact]
    public async Task SearchAddress_Line1()
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
            var response = await HttpClient.SendAsync(request);

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
    public async Task SearchAddress_Postcode()
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
            var response = await HttpClient.SendAsync(request);

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
    public async Task SearchAddress_Line1AndPostcode()
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
            var response = await HttpClient.SendAsync(request);

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
    public async Task SearchAddress_Prefix()
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
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("assets")[0];
            firstResult.GetProperty("assetAddress").GetProperty("addressLine1").GetString().Should().Contain(searchText);
        });
        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task SearchAddress_Partial()
    {
        const int maxAttempts = 10;
        const int minSuccessCount = 8;

        var successCount = await RunWithScore(maxAttempts, async () =>
        {
            // Arrange
            var randomAsset = RandomItem();
            var randomAddress = randomAsset.GetProperty("assetAddress").GetProperty("addressLine1").GetString();
            var searchTerms = randomAddress.Split(' ').ToList();
            // remove one search term
            searchTerms.RemoveAt(0);
            var searchText = string.Join(" ", searchTerms);
            var request = CreateSearchRequest(searchText);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("assets")[0];
            var firstResultAddress = firstResult.GetProperty("assetAddress").GetProperty("addressLine1").GetString();
            searchTerms.ForEach(term =>
                firstResultAddress.Should().Contain(term)
            );
        });
        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task SearchAddress_Typo()
    {
        const int maxAttempts = 10;
        const int minSuccessCount = 8;

        var successCount = await RunWithScore(maxAttempts, async () =>
        {
            // Arrange
            var randomAsset = RandomItem();
            var randomAddress = randomAsset.GetProperty("assetAddress").GetProperty("addressLine1").GetString();

            var searchText = CreateTypo(randomAddress);
            var request = CreateSearchRequest(searchText);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("assets")[0];
            firstResult.GetProperty("id").GetString().Should().Be(randomAsset.GetProperty("id").GetString());

        });
        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    #endregion

    #region Tenure

    [Fact]
    public async Task SearchTenure_PaymentRef()
    {
        // Arrange
        var asset = RandomItem();
        var searchText = asset.GetProperty("tenure").GetProperty("paymentReference").GetString();
        var request = CreateSearchRequest(searchText);

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var root = GetResponseRootElement(response);
        root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
        var firstResult = root.GetProperty("results").GetProperty("assets")[0];
        firstResult.GetProperty("tenure").GetProperty("paymentReference").GetString().Should().Be(searchText);
    }

    #endregion

    #region Asset

    [Fact]
    public async Task SearchAsset_Id()
    {
        // Arrange
        var asset = RandomItem();
        var expectedReturnedId = asset.GetProperty("id").GetString();
        var searchText = expectedReturnedId;
        var request = CreateSearchRequest(searchText);

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var root = GetResponseRootElement(response);
        root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
        var firstResult = root.GetProperty("results").GetProperty("assets")[0];
        firstResult.GetProperty("id").GetString().Should().Be(expectedReturnedId);
    }

    #endregion
}

