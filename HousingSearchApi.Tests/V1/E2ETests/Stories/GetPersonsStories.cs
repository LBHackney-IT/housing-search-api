using System.Collections.Generic;
using Hackney.Shared.HousingSearch.Domain;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps;
using TestStack.BDDfy;
using Xunit;

namespace HousingSearchApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "The Sender Search Endpoint to return results",
        SoThat = "it is possible to search for persons")]
    [Collection("ElasticSearch collection")]
    public class GetPersonsStories
    {
        private readonly MockWebApplicationFactory<Startup> _factory;
        private readonly PersonsFixture _personsFixture;
        private readonly GetPersonsSteps _steps;

        public GetPersonsStories(MockWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            var httpClient = factory.CreateClient();
            var elasticClient = factory.ElasticSearchClient;

            _steps = new GetPersonsSteps(httpClient);
            _personsFixture = new PersonsFixture(elasticClient, httpClient);
        }

        [Fact]
        public void ServiceReturnsBadResult()
        {
            this.Given(g => _personsFixture.GivenAPersonIndexExists())
                .When(w => _steps.WhenRequestDoesNotContainSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBeBadRequestResult("'Search Text' must not be empty."))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkResult()
        {
            this.Given(g => _personsFixture.GivenAPersonIndexExists())
                .When(w => _steps.WhenRequestContainsSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsCorrectPageSize()
        {
            this.Given(g => _personsFixture.GivenAPersonIndexExists())
                .When(w => _steps.WhenAPageSizeIsProvided(5))
                .Then(t => _steps.ThenTheReturningResultsShouldBeOfThatSize(5))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsExactMatchForFirstAndLastName()
        {
            var firstName = "Bob";
            var lastName = "Smith";

            this.Given(g => _personsFixture.GivenAPersonIndexExists())
                .Given(g => _personsFixture.GivenThereExistPersonsWithSimilarFirstAndLastNames(firstName, lastName))
                .When(w => _steps.WhenSearchingByFirstAndLastName(firstName, lastName))
                .Then(t => _steps.ThenTheFirstResultShouldBeAnExactMatchOfFirstNameAndLastName(firstName, lastName))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOnlyLeaseholderPersons()
        {
            this.Given(g => _personsFixture.GivenAPersonIndexExists())
                .Given(g => _personsFixture.GivenDifferentTypesOfTenureTypes("SomePersonName", "SomePersonLastName", new List<string> { "Tenant", "Leaseholder" }))
                .When(w => _steps.WhenARequestContainsSearchByTenureTypes("SomePersonLastName", new List<string> { "Tenant" }))
                .Then(t => _steps.ThenTheResultShouldContainOnlyTheSearchedTypes(new List<string> { "Tenant" }))
                .BDDfy();
        }

        [Theory]
        [InlineData("SomePersonName", "SomePersonLastName")]
        public void ServiceDoesNotReturnHousingOfficers(string firstName, string lastName)
        {
            this.Given(g => _personsFixture.GivenAPersonIndexExists())
                .Given(g => _personsFixture.GivenThereExistPersonsWithDifferentPersonTypes(firstName, lastName, new List<PersonType> { PersonType.HousingOfficer, PersonType.Tenant, PersonType.Leaseholder, PersonType.HousingOfficer }))
                .When(w => _steps.WhenSearchingByFirstAndLastName(firstName, lastName))
                .Then(t => _steps.ThenTheResultShouldNotContainHousingOfficers())
                .BDDfy();
        }
    }
}
