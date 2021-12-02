using System;
using System.Linq;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps;
using Microsoft.AspNetCore.Mvc.Testing;
using TestStack.BDDfy;
using Xunit;

namespace HousingSearchApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "The Account Search Endpoint to return results",
        SoThat = "it is possible to search for accounts")]
    [Collection("ElasticSearch collection")]
    public class GetAccountStories
    {
        private readonly MockWebApplicationFactory<Startup> _factory;
        private readonly AccountFixture _accountsFixture;
        private readonly GetAccountSteps _steps;

        public GetAccountStories(MockWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            var httpClient = factory.CreateClient();
            var elasticClient = factory.ElasticSearchClient;

            _steps = new GetAccountSteps(httpClient);
            _accountsFixture = new AccountFixture(elasticClient, httpClient);
        }

        [Fact]
        public void ServiceReturnsBadResult()
        {
            this.Given(g => _accountsFixture.GivenAnAccountIndexExists())
                .When(w => _steps.WhenRequestDoesNotContainSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBeBadRequestResult())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkResult()
        {
            this.Given(g => _accountsFixture.GivenAnAccountIndexExists())
                .When(w => _steps.WhenRequestContainsSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsCorrectPageSize()
        {
            this.Given(g => _accountsFixture.GivenAnAccountIndexExists())
                .When(w => _steps.WhenAPageSizeIsProvided(5))
                .Then(t => _steps.ThenTheReturningResultsShouldBeOfThatSize(5))
                .BDDfy();
        }

        [Fact]
        public void ServiceFiltersGivenTargetIds()
        {
            var account = AccountFixture.AccountSearchStubs.Last().TargetId;
            this.Given(g => _accountsFixture.GivenAnAccountIndexExists())
                .When(w => _steps.WhenTargetIdsAreProvided(account))
                .Then(t => _steps.ThenOnlyTheseAccountTargetIdsShouldBeIncluded(account))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsExactMatchFirstIfExists()
        {
            var accounts= AccountFixture.AccountSearchStubs.TakeLast(2);
            this.Given(g => _accountsFixture.GivenAnAccountIndexExists())
                .When(w => _steps.WhenAnExactMatchExists(accounts.First().FullAddress))
                .Then(t => _steps.ThenThatAddressShouldBeTheFirstResult(accounts.First().FullAddress))
                .BDDfy();
        }
    }
}
