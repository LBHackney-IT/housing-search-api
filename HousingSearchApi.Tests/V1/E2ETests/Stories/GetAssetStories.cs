using System.Linq;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps;
using TestStack.BDDfy;
using Xunit;

namespace HousingSearchApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "The Asset Search Endpoint to return results",
        SoThat = "it is possible to search for assets")]
    [Collection("ElasticSearch collection")]
    public class GetAssetStories
    {
        private readonly MockWebApplicationFactory<Startup> _factory;
        private readonly AssetFixture _assetsFixture;
        private readonly GetAssetSteps _steps;

        public GetAssetStories(MockWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            var httpClient = factory.CreateClient();
            var elasticClient = factory.ElasticSearchClient;

            _steps = new GetAssetSteps(httpClient);
            _assetsFixture = new AssetFixture(elasticClient, httpClient);
        }

        [Fact]
        public void ServiceReturnsBadResult()
        {
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenRequestDoesNotContainSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBeBadRequestResult(default))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkResult()
        {
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenRequestContainsSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsCorrectPageSize()
        {
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenAPageSizeIsProvided(5))
                .Then(t => _steps.ThenTheReturningResultsShouldBeOfThatSize(5))
                .BDDfy();
        }

        [Fact]
        public void ServiceFiltersGivenAssetTypes()
        {
            var asset = "NA";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenAssetTypesAreProvided(asset))
                .Then(t => _steps.ThenOnlyTheseAssetTypesShouldBeIncluded(asset))
                .BDDfy();
        }

        [Fact]
        public void ServiceFiltersGivenAssetTypesWithSearchTextStarStar()
        {
            var asset = "Dwelling";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenSearchTextProvidedAsStarStarAndAssetTypeProvidedAndLastHitIdNotProvided(asset))
                .Then(d => _steps.ThenOnlyLastHitIdShouldBeIncluded())
                .Then(t => _steps.ThenOnlyAllAssetsResponseTheseAssetTypesShouldBeIncluded(asset))
                .BDDfy();
        }

        [Fact]
        public void ServiceFiltersGivenAssetTypesWithSearchTextStarStarAndGivenLastHitId()
        {
            var asset = "Dwelling";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenSearchTextProvidedAsStarStarAndAssetTypeProvidedAndLastHitIdProvided(asset))
                .Then(t => _steps.ThenOnlyAllAssetsResponseTheseAssetTypesShouldBeIncluded(asset))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsExactMatchFirstIfExists()
        {
            var assetTypes = AssetFixture.Addresses.TakeLast(2);
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenAnExactMatchExists("59 Buckland Court St Johns Estate"))
                .Then(t => _steps.ThenThatAddressShouldBeTheFirstResult("59 Buckland Court St Johns Estate"))
                .BDDfy();
        }
    }
}
