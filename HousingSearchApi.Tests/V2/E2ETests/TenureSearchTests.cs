using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using HousingSearchApi.Tests.V2.E2ETests.Fixtures;
using Xunit.Abstractions;


namespace HousingSearchApi.Tests.V2.E2ETests;

public class TenureSearchTests : BaseSearchTests
{

    public TenureSearchTests(CombinedFixture combinedFixture) : base(combinedFixture, indexName: "tenures"){}


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
        root.GetProperty("results").GetProperty("tenures").GetArrayLength().Should().Be(0);
    }

    # endregion

    # region Address

    [Fact]
    public async Task SearchAddress_Full()
    {
        const int attempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(attempts, async () =>
        {
            // Arrange
            var randomTenure = RandomItem();
            var expectedReturnedId = randomTenure.GetProperty("id").GetString();
            var query = randomTenure.GetProperty("tenuredAsset").GetProperty("fullAddress").GetString();
            var request = CreateSearchRequest(query);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);

            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("tenures")[0];
            firstResult.GetProperty("id").GetString().Should().Be(expectedReturnedId);
        });

        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task SearchAddress_Prefix()
    {
        const int attempts = 10;
        const int minSuccessCount = 8;

        var successCount = await RunWithScore(attempts, async () =>
        {
            // Arrange
            var randomTenure = RandomItem();
            var randomAddress = randomTenure.GetProperty("tenuredAsset").GetProperty("fullAddress").GetString();
            var searchTerms = randomAddress.Split(' ').ToList();
            // remove last search term
            searchTerms = searchTerms.Where((_, index) => index != searchTerms.Count - 1).ToList();
            var searchText = string.Join(" ", searchTerms);

            var request = CreateSearchRequest(searchText);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("tenures")[0];
            var firstResultAddress = firstResult.GetProperty("tenuredAsset").GetProperty("fullAddress").GetString();
            firstResultAddress.Should().Contain(searchText);
        });

        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task SearchAddress_Partial()
    {
        const int attempts = 10;
        const int minSuccessCount = 8;

        var successCount = await RunWithScore(attempts, async () =>
        {
            // Arrange
            var randomTenure = RandomItem();
            var randomAddress = randomTenure.GetProperty("tenuredAsset").GetProperty("fullAddress").GetString();
            var searchTerms = randomAddress.Split(' ').ToList();
            // remove one search term
            searchTerms = searchTerms.Where((_, index) => index != searchTerms.Count).ToList();
            var searchText = string.Join(" ", searchTerms);

            var request = CreateSearchRequest(searchText);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("tenures")[0];
            var firstResultAddress = firstResult.GetProperty("tenuredAsset").GetProperty("fullAddress").GetString();
            // should contain each search term
            searchTerms.ForEach(term => firstResultAddress.Should().Contain(term));
        });

        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    # endregion

    # region Tenure

    [Fact]
    public async Task SearchTenure_PaymentRef()
    {
        // Arrange
        var tenure = RandomItem();
        var expectedReturnedId = tenure.GetProperty("id").GetString();
        var searchText = tenure.GetProperty("paymentReference").GetString();
        var request = CreateSearchRequest(searchText);

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var root = GetResponseRootElement(response);
        root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
        var firstResult = root.GetProperty("results").GetProperty("tenures")[0];
        firstResult.GetProperty("id").GetString().Should().Be(expectedReturnedId);
    }

    [Fact]
    public async Task SearchTenure_Id()
    {
        // Arrange
        var tenure = RandomItem();
        var expectedReturnedId = tenure.GetProperty("id").GetString();
        var searchText = tenure.GetProperty("id").GetString();
        var request = CreateSearchRequest(searchText);

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var root = GetResponseRootElement(response);
        root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
        var firstResult = root.GetProperty("results").GetProperty("tenures")[0];
        firstResult.GetProperty("id").GetString().Should().Be(expectedReturnedId);
    }

    # endregion

    # region Person

    [Fact]
    public async Task SearchPerson_Name()
    {
        const int attempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(attempts, async () =>
        {
            // Arrange
            var tenure = RandomItem();
            var expectedReturnedId = tenure.GetProperty("id").GetString();
            var householdMembers = tenure.GetProperty("householdMembers");
            var randomMember = householdMembers[Random.Next(householdMembers.GetArrayLength())];
            var searchText = randomMember.GetProperty("fullName").GetString();
            var request = CreateSearchRequest(searchText);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("tenures")[0];
            firstResult.GetProperty("id").GetString().Should().Be(expectedReturnedId);
        });

        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task SearchPerson_NameTypo()
    {
        const int attempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(attempts, async () =>
        {
            // Arrange
            var tenure = RandomItem();
            var expectedReturnedId = tenure.GetProperty("id").GetString();
            var householdMembers = tenure.GetProperty("householdMembers");
            var randomMember = householdMembers[Random.Next(householdMembers.GetArrayLength())];
            var memberName = randomMember.GetProperty("fullName").GetString();

            var searchText = CreateTypo(memberName);
            var request = CreateSearchRequest(searchText);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            // assert the result is in the first 5 results
            var results = root.GetProperty("results").GetProperty("tenures");
            var resultIds = results.EnumerateArray().Select(r => r.GetProperty("id").GetString()).ToList();
            resultIds.Should().Contain(expectedReturnedId);
        });

        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task SearchPerson_NameMissingTitle()
    {
        const int attempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(attempts, async () =>
        {
            // Arrange
            var tenure = RandomItem();
            var expectedReturnedId = tenure.GetProperty("id").GetString();
            var householdMembers = tenure.GetProperty("householdMembers");
            var randomMember = householdMembers[Random.Next(householdMembers.GetArrayLength())];
            var memberName = randomMember.GetProperty("fullName").GetString();
            var nameParts = memberName.Split(' ').ToList();

            // Remove the title
            nameParts.RemoveAt(0);
            var searchText = string.Join(" ", nameParts);
            var request = CreateSearchRequest(searchText);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("tenures")[0];
            firstResult.GetProperty("id").GetString().Should().Be(expectedReturnedId);
        });

        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    # endregion
}

