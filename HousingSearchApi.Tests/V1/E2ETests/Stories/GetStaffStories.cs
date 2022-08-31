using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps;
using TestStack.BDDfy;
using Xunit;

namespace HousingSearchApi.Tests.V1.E2ETests.Stories
{
    [Story(
       AsA = "Service",
       IWant = "The Staff Search Endpoint to return results",
       SoThat = "it is possible to search for staffs")]
    [Collection("ElasticSearch collection")]
    public class GetStaffStories
    {
        private readonly MockWebApplicationFactory<Startup> _factory;
        private readonly StaffFixture _staffsFixture;
        private readonly GetStaffsSteps _steps;

        public GetStaffStories(MockWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            var httpClient = factory.CreateClient();
            var elasticClient = factory.ElasticSearchClient;

            _steps = new GetStaffsSteps(httpClient);
            _staffsFixture = new StaffFixture(elasticClient, httpClient);
        }

        [Fact]
        public void ServiceReturnsBadResult()
        {
            this.Given(g => _staffsFixture.GivenAnStaffIndexExists())
                .When(w => _steps.WhenRequestDoesNotContainSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBeBadRequestResult(default))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkResult()
        {
            this.Given(g => _staffsFixture.GivenAnStaffIndexExists())
                .When(w => _steps.WhenRequestContainsSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsCorrectPageSize()
        {
            this.Given(g => _staffsFixture.GivenAnStaffIndexExists())
                .When(w => _steps.WhenAPageSizeIsProvided(5))
                .Then(t => _steps.ThenTheReturningResultsShouldBeOfThatSize(5))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsExactMatchForEmailAddress()
        {
            var emailAddress = "firstname3.lastname3@test.com";

            this.Given(g => _staffsFixture.GivenAnStaffIndexExists())
                .Given(g => _staffsFixture.GivenThereExistPersonsWithSimilarEmailAddress(emailAddress))
                .When(w => _steps.WhenSearchingByEmailAddress(emailAddress))
                .Then(t => _steps.ThenTheFirstResultShouldBeAnExactMatchOfEmailAddress(emailAddress))
                .BDDfy();
        }


    }
}
