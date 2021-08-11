using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentAssertions;
using HousingSearchApi.Tests.V1.Helper;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Domain;
using Xunit;

namespace HousingSearchApi.Tests
{
    [Collection("ElasticSearch collection")]
    public class IntegrationTests
    {
        private readonly ElasticSearchFixture _elasticSearchFixture;
        private HttpClient Client => _elasticSearchFixture.Client;
        protected readonly JsonSerializerOptions _jsonOptions;

        public IntegrationTests(ElasticSearchFixture elasticSearchFixture)
        {
            _elasticSearchFixture = elasticSearchFixture;
            _jsonOptions = CreateJsonOptions();
        }

        private static JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }

        [Fact]
        public async Task WhenRequestDoesNotContainSearchStringShouldReturnBadRequestResult()
        {
            // arrange + act
            var response = await Client.GetAsync(new Uri("api/v1/search/persons", UriKind.Relative)).ConfigureAwait(false);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WhenRequestContainsSearchStringShouldReturn200()
        {
            // arrange + act
            var response = await Client.GetAsync(new Uri("api/v1/search/persons?searchText=abc", UriKind.Relative)).ConfigureAwait(false);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task WhenRequestGetsResultMaxPageSizeWouldBeTheOneRequestedInTheQueryString()
        {
            // arrange +
            var pageSize = 5;
            var personType = PersonType.Rent;

            // act
            var route = new Uri($"api/v1/search/persons?searchText={TestDataHelper.Alphabet.Last()}&pageSize={pageSize}&personType={personType}",
                                UriKind.Relative);
            var response = await Client.GetAsync(route).ConfigureAwait(false);

            // assert
            var resultBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetPersonListResponse>>(resultBody, _jsonOptions);

            result.Results.Persons.Count.Should().Be(pageSize);
        }

        [Fact]
        public async Task WhenRequestContainsSearchStringAndSortingLastNameAscShouldReturn200AndSortAppropriately()
        {
            // act
            var route = new Uri($"api/v1/search/persons?searchText={TestDataHelper.Alphabet.Last()}&sortBy=surname&isDesc=false&pageSize=10000",
                                UriKind.Relative);
            var response = await Client.GetAsync(route).ConfigureAwait(false);

            // assert
            var resultBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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

        [Fact]
        public async Task WhenRequestContainsSearchStringAndSortingLastNameDescShouldReturn200AndSortAppropriately()
        {
            // arrange + act
            var route = new Uri($"api/v1/search/persons?searchText={TestDataHelper.Alphabet.Last()}&sortBy=surname&isDesc=true&pageSize=10000",
                                UriKind.Relative);
            var response = await Client.GetAsync(route).ConfigureAwait(false);

            // assert
            var resultBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetPersonListResponse>>(resultBody, _jsonOptions);

            for (int i = 1; i < result.Results.Persons.Count; i++)
            {
                // if i == to the last element, then break
                if (i + 1 == result.Results.Persons.Count)
                    break;

                // We are comparing the first char of the first sorted person to the first char of every other person. It should be greater than
                // or equal to the next in line for desc
                result.Results.Persons[i].Surname.ToCharArray().First().Should().BeGreaterOrEqualTo(result.Results.Persons[i + 1].Surname.ToCharArray().First());
            }
        }
    }
}
