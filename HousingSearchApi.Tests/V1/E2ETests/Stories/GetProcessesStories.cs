using Hackney.Shared.HousingSearch.Domain.Process;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps;
using System;
using TestStack.BDDfy;
using Xunit;

namespace HousingSearchApi.Tests.V1.E2ETests.Stories
{
    [Story(
    AsA = "Service",
    IWant = "The Asset Search Endpoint to return results",
    SoThat = "it is possible to search for assets")]
    [Collection("ElasticSearch collection")]
    public class GetProcessesStories
    {
        private readonly MockWebApplicationFactory<Startup> _factory;
        private readonly ProcessFixture _processesFixture;
        private readonly GetProcessesSteps _steps;

        public GetProcessesStories(MockWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            var httpClient = factory.CreateClient();
            var elasticClient = factory.ElasticSearchClient;

            _steps = new GetProcessesSteps(httpClient);
            _processesFixture = new ProcessFixture(elasticClient, httpClient);
        }

        [Fact]
        public void ServiceReturnsBadResult()
        {
            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenRequestDoesNotContainSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBeBadRequestResult(default))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkResult()
        {
            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenRequestContainsSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceFiltersGivenTargetTypeAndTargetId()
        {
            var targetType = TargetType.tenure;
            var targetId = ProcessFixture.Processes[2].TargetId;
            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenTargetTypeAndTargetIdAreProvided(targetType, targetId))
                .Then(t => _steps.ThenOnlyTheseTargetTypeAndTargetIdShouldBeIncluded(targetType, targetId))
                .BDDfy();
        }
    }
}
