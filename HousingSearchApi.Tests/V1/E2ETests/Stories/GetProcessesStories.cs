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
            ProcessFixture.Processes[2].PatchAssignment.PatchId = Guid.NewGuid().ToString();
            var patchId = ProcessFixture.Processes[2].PatchAssignment.PatchId;

            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenRequestContainsSearchString(patchId))
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .Then(t => _steps.ThenTheFirstResultShouldBeAnExactMatchOfPatchId(patchId))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkResultWithBothTargetIdAndTargetType()
        {
            var targetId = ProcessFixture.Processes[2].TargetId;
            var targetType = ProcessFixture.Processes[2].TargetType;
            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenRequestContainsBothTargetIdAndTargetType(targetId, targetType))
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .Then(t => _steps.ThenTheFirstResultShouldBeAnExactMatchOfTargetId(targetId))
                .BDDfy();
        }

        [Fact]
        public void ServiceFiltersGivenTargetTypeAndTargetId()
        {
            var targetType = "tenure";
            var targetId = ProcessFixture.Processes[2].TargetId;
            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenTargetTypeAndTargetIdAreProvided(targetType, targetId))
                .Then(t => _steps.ThenOnlyTheseTargetTypeAndTargetIdShouldBeIncluded(targetType, targetId))
                .BDDfy();
        }

        [Fact]
        public void ServiceFiltersGivenProcessName()
        {
            var processName = "soletojoint";
            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenProcessNameIsProvided(processName))
                .Then(t => _steps.ThenOnlyTheProcessNameShouldBeIncluded(processName))
                .BDDfy();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ServiceFiltersByOpenStatus(bool isOpen)
        {
            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenProcessStatusIsProvided(isOpen))
                .Then(t => _steps.ThenOnlyTheProcessStatusShouldBeIncluded(isOpen))
                .BDDfy();
        }


        [Fact]
        public void ServiceFailWhenOnlyTargetTypeIsGiven()
        {
            var targetType = "tenure";
            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenTargetTypeIsProvided(targetType))
                .Then(t => _steps.ThenTheLastRequestShouldBeBadRequestResult(default))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsCorrectPageSize()
        {
            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenAPageSizeIsProvided(3))
                .Then(t => _steps.ThenTheReturningResultsShouldBeOfThatSize(3))
                .BDDfy();
        }

        [Theory]
        [InlineData("name")]
        [InlineData("process")]
        [InlineData("patch")]
        [InlineData("state")]
        public void ServiceReturnsCorrectAscSort(string sortBy)
        {
            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenARequestIsSortedByAFieldAsc(sortBy, false))
                .Then(t => _steps.ThenTheResultShouldBeSortedAsc(sortBy, false, ProcessFixture.Processes))
                .BDDfy();
        }

        [Theory]
        [InlineData("name")]
        [InlineData("process")]
        [InlineData("patch")]
        [InlineData("state")]
        public void ServiceReturnsCorrectDescSort(string sortBy)
        {
            this.Given(g => _processesFixture.GivenAnProcessIndexExists())
                .When(w => _steps.WhenARequestIsSortedByAFieldAsc(sortBy, true))
                .Then(t => _steps.ThenTheResultShouldBeSortedDesc(sortBy, true, ProcessFixture.Processes))
                .BDDfy();
        }
    }
}
