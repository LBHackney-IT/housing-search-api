using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using HousingSearchApi.Tests.V2.E2ETests.Fixtures;
using NUnit.Framework;


namespace HousingSearchApi.Tests.V2.E2ETests;

public class PersonSearchTests : BaseSearchTests
{

    public PersonSearchTests(CombinedFixture combinedFixture) : base(combinedFixture, indexName: "persons") { }


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
        root.GetProperty("results").GetProperty("persons").GetArrayLength().Should().Be(0);
    }

    [Fact]
    public async Task Search_FilterFields()
    {
        string[] testTitles = ["Dr", "Mrs"];
        string[] expectedPersonIds =
        [
            "b69e6519-3712-4509-a30e-0b5af596915b",
            "ad75b5d2-9d2d-44d6-a739-37cdd5d358e7",
        ];
        string[] requestedPersonIds =
        [
            .. expectedPersonIds,
            "a79b15c8-289e-4410-9397-720414ab6db9",
        ];
        var request = CreateSearchRequest(
            "Turnbull Neck",
            ("title", "dr"),
            ("title", "mrs"),
            ("id", requestedPersonIds[0]),
            ("id", requestedPersonIds[1]),
            ("id", requestedPersonIds[2])
        );

        var response = await HttpClient.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var root = GetResponseRootElement(response);
        var results = root.GetProperty("results").GetProperty("persons").EnumerateArray().ToList();
        results.Should().NotBeEmpty();
        results.Should().OnlyContain(person =>
            testTitles.Contains(person.GetProperty("title").GetString())
            && expectedPersonIds.Contains(person.GetProperty("id").GetString())
        );
        results.Select(person => person.GetProperty("title").GetString())
            .Should().Contain(testTitles);
        results.Select(person => person.GetProperty("id").GetString())
            .Should().Contain(expectedPersonIds);
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
            var randomPerson = RandomItem();
            var expectedReturnedId = randomPerson.GetProperty("id").GetString();
            var query = randomPerson.GetProperty("tenures")[0].GetProperty("assetFullAddress").GetString();
            var request = CreateSearchRequest(query);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);

            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("persons")[0];
            firstResult.GetProperty("tenures")[0].GetProperty("assetFullAddress").GetString().Should().Be(query);
            firstResult.GetProperty("confidenceScore").GetDouble().Should().BeGreaterThan(0);
        });

        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }


    [Fact]
    public async Task SearchAddress_Partial()
    {
        const int attempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(attempts, async () =>
        {
            // Arrange
            var randomPerson = RandomItem();
            var expectedReturnedId = randomPerson.GetProperty("id").GetString();
            var randomAddress = randomPerson.GetProperty("tenures")[0].GetProperty("assetFullAddress").GetString();
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
            var firstResults = root.GetProperty("results").GetProperty("persons");
            firstResults.EnumerateArray().Take(5).Any(result =>
            {
                return result.GetProperty("id").GetString() == expectedReturnedId;
            }).Should().BeTrue();
            firstResults[0].GetProperty("confidenceScore").GetDouble().Should().BeGreaterThan(0);
        });

        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }


    [Fact]
    public async Task SearchAddress_Typo()
    {
        const int attempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(attempts, async () =>
        {
            // Arrange
            var randomPerson = RandomItem();
            var expectedReturnedId = randomPerson.GetProperty("id").GetString();
            var randomAddress = randomPerson.GetProperty("tenures")[0].GetProperty("assetFullAddress").GetString();
            var query = CreateTypo(randomAddress);
            var request = CreateSearchRequest(query);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);

            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResults = root.GetProperty("results").GetProperty("persons");
            firstResults.EnumerateArray().Take(5).Any(result =>
            {
                return result.GetProperty("id").GetString() == expectedReturnedId;
            }).Should().BeTrue();
            firstResults[0].GetProperty("confidenceScore").GetDouble().Should().BeGreaterThan(0);
        });

        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    # endregion

    # region Tenure

    [Fact]
    public async Task SearchTenure_PaymentRef()
    {
        // Arrange
        var person = RandomItem();
        var searchText = person.GetProperty("tenures")[0].GetProperty("paymentReference").GetString();
        var request = CreateSearchRequest(searchText);

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var root = GetResponseRootElement(response);
        try
        {
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("persons")[0];
            firstResult.GetProperty("tenures")[0].GetProperty("paymentReference").GetString().Trim().Should().Be(searchText.Trim());
            firstResult.GetProperty("confidenceScore").GetDouble().Should().BeGreaterThan(0);
        }
        catch (AssertionException e)
        {
            var errMsg = $"Failed to assert that the payment reference is {searchText}";
            throw new AssertionException(errMsg + "\n" + e.Message);
        }
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
            var person = RandomItem();
            var expectedReturnedId = person.GetProperty("id").GetString();
            var searchText = person.GetProperty("firstname").GetString() + " " + person.GetProperty("surname").GetString();
            var request = CreateSearchRequest(searchText);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResult = root.GetProperty("results").GetProperty("persons")[0];
            firstResult.GetProperty("id").GetString().Should().Be(expectedReturnedId);
            firstResult.GetProperty("confidenceScore").GetDouble().Should().BeGreaterThan(0);
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
            var person = RandomItem();
            var expectedReturnedId = person.GetProperty("id").GetString();
            var memberName = person.GetProperty("firstname").GetString() + " " + person.GetProperty("surname").GetString();
            var searchText = CreateTypo(memberName);
            var request = CreateSearchRequest(searchText);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var firstResults = root.GetProperty("results").GetProperty("persons").EnumerateArray().Take(5);
            firstResults.Any(result =>
                result.GetProperty("id").GetString() == expectedReturnedId
            ).Should().BeTrue();
            root.GetProperty("results").GetProperty("persons")[0].GetProperty("confidenceScore").GetDouble().Should().BeGreaterThan(0);
        });

        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    [Fact]
    public async Task SearchPerson_NamePartRemoved()
    {
        const int attempts = 10;
        const int minSuccessCount = 9;

        var successCount = await RunWithScore(attempts, async () =>
        {
            // Arrange
            var person = RandomItem();
            var expectedReturnedId = person.GetProperty("id").GetString();
            var memberName = person.GetProperty("firstname").GetString() + " " + person.GetProperty("surname").GetString();
            var nameParts = memberName.Split(' ');
            // Remove a random name part (i.e. firstname, "middle name", or surname)
            var randomIndexInNameParts = Random.Next(nameParts.Length);
            nameParts = nameParts.Where((_, index) => index != randomIndexInNameParts).ToArray();
            var searchText = string.Join(" ", nameParts);

            var request = CreateSearchRequest(searchText);

            // Act
            var response = await HttpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var root = GetResponseRootElement(response);
            root.GetProperty("total").GetInt32().Should().BeGreaterThan(0);
            var results = root.GetProperty("results").GetProperty("persons");
            // checking the first few avoids flakiness due to matching addresses which contain names
            results.EnumerateArray().Take(3).Any(result =>
                {
                    return nameParts.All(part => result.GetProperty("firstname").GetString().Contains(part) || result.GetProperty("surname").GetString().Contains(part));
                }).Should().BeTrue();
            results[0].GetProperty("confidenceScore").GetDouble().Should().BeGreaterThan(0);
        });

        successCount.Should().BeGreaterThanOrEqualTo(minSuccessCount);
    }

    # endregion
}

