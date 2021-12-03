using System.Linq;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps;
using TestStack.BDDfy;
using Xunit;

namespace HousingSearchApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "The Transactions Search Endpoint to return results",
        SoThat = "it is possible to search for transactions")]
    [Collection("ElasticSearch collection")]
    public class GetTransactionsStories
    {
        private readonly TransactionsFixture _transactionsFixture;
        private readonly GetTransactionsSteps _transactionsSteps;

        public GetTransactionsStories(MockWebApplicationFactory<Startup> factory)
        {
            var httpClient = factory.CreateClient();
            var elasticClient = factory.ElasticSearchClient;

            _transactionsSteps = new GetTransactionsSteps(httpClient);
            _transactionsFixture = new TransactionsFixture(elasticClient, httpClient);
        }

        [Fact]

        public void ServiceReturnsBadResult()
        {
            this.Given(_ => _transactionsFixture.GivenAnAssetIndexExists())
                .When(_ => _transactionsSteps.WhenRequestDoesNotContainSearchString())
                .Then(_ => _transactionsSteps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkWithData()
        {
            this.Given(_ => _transactionsFixture.GivenAnAssetIndexExists())
                .When(_ => _transactionsSteps.WhenRequestContainsSearchText("some wrong search string"))
                .Then(_ => _transactionsSteps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkWithExactPageSize()
        {
            this.Given(_ => _transactionsFixture.GivenAnAssetIndexExists())
                .When(_ => _transactionsSteps.WhenAPageSizeIsProvided(10))
                .Then(_ => _transactionsSteps.ThenTheReturningResultsShouldBeOfThatSize(10))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkWithMatchesByFullName()
        {
            this.Given(_ => _transactionsFixture.GivenAnAssetIndexExists())
                .When(_ => _transactionsSteps.WhenRequestContainsSearchText(TransactionsFixture.Senders.First().FullName))
                .Then(_ => _transactionsSteps.ThenThatTextShouldBeInTheResult(TransactionsFixture.Senders.First().FullName))
                .BDDfy();
        }
    }
}
