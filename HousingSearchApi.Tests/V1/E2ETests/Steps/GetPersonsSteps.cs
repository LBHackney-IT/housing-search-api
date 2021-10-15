using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps.Base;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Infrastructure;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps
{
    public class GetPersonsSteps : BaseSteps
    {
        public GetPersonsSteps(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task WhenRequestDoesNotContainSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/persons", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenRequestContainsSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/persons?searchText=abc", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenSearchingByFirstAndLastName(string firstName, string lastName)
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"api/v1/search/persons?searchText=%20{firstName}%20{lastName}", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenAPageSizeIsProvided(int pageSize)
        {
            var route = new Uri($"api/v1/search/persons?searchText={PersonsFixture.Alphabet.Last()}&pageSize={pageSize}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenARequestIsSortedByAFieldAsc()
        {
            var route = new Uri($"api/v1/search/persons?searchText={PersonsFixture.Alphabet.Last()}&sortBy=surname&isDesc=false&pageSize=1000",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenARequestIsSortedByAFieldDesc()
        {
            var route = new Uri($"api/v1/search/persons?searchText={PersonsFixture.Alphabet.Last()}&sortBy=surname&isDesc=true&pageSize=1000",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenARequestContainsSearchByTenant()
        {
            var route = new Uri($"api/v1/search/persons?searchText={PersonsFixture.Alphabet.Last()}&personType=Tenant",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenARequestContainsSearchByLeaseholder()
        {
            var route = new Uri($"api/v1/search/persons?searchText={PersonsFixture.Alphabet.Last()}&personType=Leaseholder",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task ThenTheLastRequestShouldBeBadRequestResult()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            resultBody.Should().NotBeNull();
            resultBody.Should().Contain("'Search Text' must not be empty.");
        }

        public void ThenTheLastRequestShouldBe200()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public async Task ThenTheReturningResultsShouldBeOfThatSize(int pageSize)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetPersonListResponse>>(resultBody, _jsonOptions);

            result.Results.Persons.Count.Should().Be(pageSize);
        }

        public async Task ThenTheFirstResultShouldBeAnExactMatchOfFirstNameAndLastName(string firstName, string lastName)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetPersonListResponse>>(resultBody, _jsonOptions);

            result.Results.Persons.First().Firstname.Should().Be(firstName);
            result.Results.Persons.First().Surname.Should().Be(lastName);
        }

        public async Task ThenTheResultShouldBeSortedAsc()
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetPersonListResponse>>(resultBody, _jsonOptions);

            for (int i = 0; i < result.Results.Persons.Count; i++)
            {
                // if i == to the last element, then break
                if (i + 1 == result.Results.Persons.Count)
                    break;

                // We are comparing the first char of the first sorted person to the first char of every other person. It should be less than
                // or equal to the next in line for asc
                result.Results.Persons[i].Surname.ToCharArray().First().Should().BeLessOrEqualTo(result.Results.Persons[i + 1].Surname.ToCharArray().First());
            }
        }

        public async Task ThenTheResultShouldBeSortedDesc()
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetPersonListResponse>>(resultBody, _jsonOptions);

            for (int i = 0; i < result.Results.Persons.Count; i++)
            {
                // if i == to the last element, then break
                if (i + 1 == result.Results.Persons.Count)
                    break;

                // We are comparing the first char of the first sorted person to the first char of every other person. It should be less than
                // or equal to the next in line for asc
                result.Results.Persons[i].Surname.ToCharArray().First().Should().BeGreaterOrEqualTo(result.Results.Persons[i + 1].Surname.ToCharArray().First());
            }
        }

        public async Task ThenTheResultShouldContainOnlyType(PersonType expectedPersonType)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetPersonListResponse>>(resultBody, _jsonOptions);

            result.Should().NotBeNull();
            result.Results.Should().NotBeNull();
            result.Results.Persons.Should().NotBeNull();

            foreach (var person in result.Results.Persons)
            {
                person.Should().NotBeNull();
                person.Tenures.Should().NotBeNull();

                person.Tenures.All(t => t.Type.IsPersonTypeOf(expectedPersonType));
            }
        }

        public async Task WhenARequestContainsSearchByTenureTypes(string somepersonlastname, List<string> tenureTypes)
        {
            var route = new Uri($"api/v1/search/persons?searchText={somepersonlastname}&personType={string.Join(" ", tenureTypes)}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task ThenTheResultShouldContainOnlyTheSearchedTypes(List<string> list)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetPersonListResponse>>(resultBody, _jsonOptions);

            foreach (var personResult in result.Results.Persons)
            {
                foreach (var tenure in personResult.Tenures)
                {
                    list.Any(x => x == tenure.Type).Should().BeTrue();
                }
            }
        }
    }
}
