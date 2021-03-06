using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps.Base;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Boundary.Responses.Metadata;

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

        public void ThenTheLastRequestShouldBeBadRequestResult()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
    }
}
