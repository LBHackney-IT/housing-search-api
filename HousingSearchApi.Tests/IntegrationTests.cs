using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HousingSearchApi.Tests.V1.Helper;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using Newtonsoft.Json;
using Xunit;

namespace HousingSearchApi.Tests
{
    [Collection("ElasticSearch collection")]
    public class IntegrationTests
    {
        protected HttpClient Client { get; private set; }
        private readonly MockWebApplicationFactory<Startup> _factory;

        public IntegrationTests()
        {
            _factory = new MockWebApplicationFactory<Startup>();
            Client = _factory.CreateClient();
        }

        [Fact(Skip = "Integration tests currently not working as they need the persons index set up in elastic search")]
        public async Task WhenRequestDoesNotContainSearchStringShouldReturnBadRequestResult()
        {
            // arrange + act
            var response = await Client.GetAsync(new Uri("api/v1/search/persons")).ConfigureAwait(false);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact(Skip = "Integration tests currently not working as they need the persons index set up in elastic search")]
        public async Task WhenRequestContainsSearchStringShouldReturn200()
        {
            // arrange + act
            var response = await Client.GetAsync(new Uri("api/v1/search/persons?searchText=abc")).ConfigureAwait(false); ;

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(Skip = "Integration tests currently not working as they need the persons index set up in elastic search")]
        public async Task WhenRequestGetsResultMaxPageSizeWouldBeTheOneRequestedInTheQueryString()
        {
            // arrange +
            var pageSize = 5;

            // act
            var response = await Client.GetAsync(new Uri($"api/v1/search/persons?searchText={TestDataHelper.Alphabet.Last()}&pageSize={pageSize}"))
                .ConfigureAwait(false); ;

            // assert
            var result = JsonConvert.DeserializeObject<APIResponse<GetPersonListResponse>>(response.Content.ReadAsStringAsync().Result);
            result.Results.Persons.Count.Should().Be(pageSize);
        }

        [Fact(Skip = "Integration tests currently not working as they need the persons index set up in elastic search")]
        public async Task WhenRequestContainsSearchStringAndSortingLastNameAscShouldReturn200AndSortAppropriately()
        {
            // act
            var response = await Client.GetAsync(new Uri($"api/v1/search/persons?searchText={TestDataHelper.Alphabet.Last()}&sortBy=surname&isDesc=false&pageSize=10000"))
                .ConfigureAwait(false);

            // assert
            var result = JsonConvert.DeserializeObject<APIResponse<GetPersonListResponse>>(response.Content.ReadAsStringAsync().Result);

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

        [Fact(Skip = "Integration tests currently not working as they need the persons index set up in elastic search")]
        public async Task WhenRequestContainsSearchStringAndSortingLastNameDescShouldReturn200AndSortAppropriately()
        {
            // arrange + act
            var response = await Client.GetAsync(new Uri($"api/v1/search/persons?searchText={TestDataHelper.Alphabet.Last()}&sortBy=surname&isDesc=true&pageSize=10000"))
                .ConfigureAwait(false);

            // assert
            var result = JsonConvert.DeserializeObject<APIResponse<GetPersonListResponse>>(response.Content.ReadAsStringAsync().Result);

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
