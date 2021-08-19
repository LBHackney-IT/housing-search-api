using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps;
using TestStack.BDDfy;
using Xunit;

namespace HousingSearchApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "The Tenure Search Endpoint to return results",
        SoThat = "it is possible to search for persons")]
    [Collection("ElasticSearch collection")]
    public class GetTenureStories
    {
        private readonly MockWebApplicationFactory<Startup> _factory;
        private readonly TenureFixture _personsFixture;
        private readonly GetPersonsSteps _steps;

        public GetTenureStories(MockWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            var httpClient = factory.CreateClient();
            var elasticClient = factory.ElasticSearchClient;

            _steps = new GetPersonsSteps(httpClient);
            _personsFixture = new TenureFixture(elasticClient, httpClient);
        }

        [Fact]
        public void ServiceReturnsBadResult()
        {
            this.Given(g => _personsFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenRequestDoesNotContainSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBeBadRequestResult())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkResult()
        {
            this.Given(g => _personsFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenRequestContainsSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsCorrectPageSize()
        {
            this.Given(g => _personsFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenAPageSizeIsProvided(5))
                .Then(t => _steps.ThenTheReturningResultsShouldBeOfThatSize(5))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsCorrectAscSort()
        {
            this.Given(g => _personsFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenARequestIsSortedByAFieldAsc())
                .Then(t => _steps.ThenTheResultShouldBeSortedAsc())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsCorrectDescSort()
        {
            this.Given(g => _personsFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenARequestIsSortedByAFieldDesc())
                .Then(t => _steps.ThenTheResultShouldBeSortedDesc())
                .BDDfy();
        }
    }
}
