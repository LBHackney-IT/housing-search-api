using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps;
using TestStack.BDDfy;
using Xunit;

namespace HousingSearchApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "The Tenure Search Endpoint to return results",
        SoThat = "it is possible to search for tenures")]
    [Collection("ElasticSearch collection")]
    public class GetTenureStories
    {
        private readonly MockWebApplicationFactory<Startup> _factory;
        private readonly TenureFixture _tenureFixture;
        private readonly GetTenureSteps _steps;

        public GetTenureStories(MockWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            var httpClient = factory.CreateClient();
            var elasticClient = factory.ElasticSearchClient;

            _steps = new GetTenureSteps(httpClient);
            _tenureFixture = new TenureFixture(elasticClient, httpClient);
        }

        [Fact]
        public void ServiceReturnsBadResult()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenRequestDoesNotContainSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBeBadRequestResult())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkResult()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenRequestContainsSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsCorrectPageSize()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenAPageSizeIsProvided(5))
                .Then(t => _steps.ThenTheReturningResultsShouldBeOfThatSize(5))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsMostRelevantResultFirst()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenSimilarTenures("FirstEntry", "SecondEntry", "ThirdEntry"))
                .When(w => _steps.WhenSearchingForASpecificTenure("FirstEntry", "SecondEntry", "ThirdEntry"))
                .Then(t => _steps.ThenTheFirstOfTheReturningResultsShouldBeTheMostRelevantOne("FirstEntry", "SecondEntry", "ThirdEntry"))
                .BDDfy();
        }
    }
}
