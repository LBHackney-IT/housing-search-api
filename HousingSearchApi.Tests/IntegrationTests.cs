using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Docker.DotNet;
using HousingSearchApi.Tests.V1.Helper;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Newtonsoft.Json;
using Xunit;

namespace HousingSearchApi.Tests
{
    public class ESFixture : IDisposable
    {
        private MockWebApplicationFactory<Startup> _factory;

        public ESFixture()
        {
            _factory = new MockWebApplicationFactory<Startup>();
            TestDataHelper.InsertPersonInEs(_factory.Services.GetService<IElasticClient>());

            // For the index to have time to be populated
            Thread.Sleep(500);
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }

    [CollectionDefinition("ES collection")]
    public class DatabaseCollection : ICollectionFixture<ESFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    [Collection("ES collection")]
    public class IntegrationTests
    {
        protected HttpClient Client { get; private set; }
        private MockWebApplicationFactory<Startup> _factory;
        private DockerClient _dockerClient;

        public IntegrationTests()
        {
            _factory = new MockWebApplicationFactory<Startup>();
            Client = _factory.CreateClient();
        }

        [Fact]
        public async Task WhenRequestDoesNotContainSearchStringShouldReturnBadRequestResult()
        {
            // arrange + act
            var response = await Client.GetAsync("api/v1/search/persons");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WhenRequestContainsSearchStringShouldReturn200()
        {
            // arrange + act
            var response = await Client.GetAsync("api/v1/search/persons?searchText=abc");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task WhenRequestGetsResultMaxPageSizeWouldBeTheOneRequestedInTheQueryString()
        {
            // arrange +
            var pageSize = 5;

            // act
            var response = await Client.GetAsync($"api/v1/search/persons?searchText={TestDataHelper.Alphabet.Last()}&pageSize={pageSize}");

            // assert
            var result = JsonConvert.DeserializeObject<APIResponse<GetPersonListResponse>>(response.Content.ReadAsStringAsync().Result);
            result.Results.Persons.Count.Should().Be(pageSize);
        }

        [Fact]
        public async Task WhenRequestContainsSearchStringAndSortingLastNameAscShouldReturn200AndSortAppropriately()
        {
            // act
            var response = await Client.GetAsync($"api/v1/search/persons?searchText={TestDataHelper.Alphabet.Last()}&sortBy=surname&isDesc=false&pageSize=10000");

            // assert
            var result = JsonConvert.DeserializeObject<APIResponse<GetPersonListResponse>>(response.Content.ReadAsStringAsync().Result);
            var firstSortedPerson = result.Results.Persons.First();

            for (int i = 1; i < result.Results.Persons.Count; i++)
            {
                firstSortedPerson.Surname.ToCharArray().First().Should().BeLessOrEqualTo(result.Results.Persons[i].Surname.ToCharArray().First());
            }
        }

        [Fact]
        public async Task WhenRequestContainsSearchStringAndSortingLastNameDescShouldReturn200AndSortAppropriately()
        {
            // arrange + act
            var response = await Client.GetAsync($"api/v1/search/persons?searchText={TestDataHelper.Alphabet.Last()}&sortBy=surname&isDesc=true&pageSize=10000");

            // assert
            var result = JsonConvert.DeserializeObject<APIResponse<GetPersonListResponse>>(response.Content.ReadAsStringAsync().Result);
            var firstSortedPerson = result.Results.Persons.First();

            for (int i = 1; i < result.Results.Persons.Count; i++)
            {
                firstSortedPerson.Surname.ToCharArray().First().Should().BeGreaterOrEqualTo(result.Results.Persons[i].Surname.ToCharArray().First());
            }
        }
    }
}
