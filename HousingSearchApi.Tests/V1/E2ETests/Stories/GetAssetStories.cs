using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps;
using Hackney.Shared.HousingSearch.Domain.Enums;
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
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenAnExactMatchExists("5 Buckland Court St Johns Estate"))
                .Then(t => _steps.ThenThatAddressShouldBeTheFirstResult("5 Buckland Court St Johns Estate"))
                .BDDfy();
        }

        [Fact]
        public void ServiceFiltersGivenAssetStatusWithoutSearchText()
        {
            var asset = "Reserved";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenAnAssetStatusIsProvided(asset))
                .Then(t => _steps.ThenOnlyAllAssetsResponseTheseAssetStatusesShouldBeIncluded(asset))
                .BDDfy();
        }

        [Fact]
        public void ServiceFiltersGivenNumberOfBedSpacesWithoutSearchText()
        {
            var bedspaces = 2;
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenNoOfBedSpacesIsProvided(bedspaces))
                .Then(t => _steps.ThenNumberOfBedSpacesShouldBeInResult(bedspaces))
                .BDDfy();
        }

        [Fact]
        public void ServiceFiltersGivenFloorNoWithoutSearchText()
        {
            var floorNo = "1";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenFloorNoIsProvided(floorNo))
                .Then(t => _steps.ThenFloorNoShouldBeInResult(floorNo))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsExactMatchOnlyWhenIsFilteredQueryTrue()
        {
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenAnExactMatchExistsAndIsFilteredQueryTrue("N1 6TY"))
                .Then(t => _steps.ThenThatAddressShouldBeTheOnlyResult("N1 6TY"))
                .BDDfy();
        }

        [Fact]
        public void ServiceFiltersTrueContractIsApprovedStatusWithoutSearchText()
        {
            var contractIsApproved = "true";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenContractIsApprovedIsProvided(contractIsApproved))
                .Then(t => _steps.ThenAssetsWithContractApprovalStatusShouldBeIncluded(contractIsApproved, 2))
                .BDDfy();
        }
        [Fact]
        public void ServiceFiltersPendingApprovalStatusWithoutSearchText()
        {
            var pendingApprovalStatus = "0";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenContractApprovalStatusIsProvided(pendingApprovalStatus))
                .Then(t => _steps.ThenAssetsWithProvidedContractApprovalStatusShouldBeIncluded(pendingApprovalStatus, 3))
                .BDDfy();
        }
        [Fact]
        public void ServiceFiltersApprovedApprovalStatusWithoutSearchText()
        {
            var approvedApprovalStatus = "1";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenContractApprovalStatusIsProvided(approvedApprovalStatus))
                .Then(t => _steps.ThenAssetsWithProvidedContractApprovalStatusShouldBeIncluded(approvedApprovalStatus, 2))
                .BDDfy();
        }
        [Fact]
        public void ServiceFiltersPendingReapprovalStatusWithoutSearchText()
        {
            var pendingReapprovalApprovalStatus = "2";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenContractApprovalStatusIsProvided(pendingReapprovalApprovalStatus))
                .Then(t => _steps.ThenAssetsWithProvidedContractApprovalStatusShouldBeIncluded(pendingReapprovalApprovalStatus, 8))
                .BDDfy();
        }
        [Fact]
        public void ServiceFiltersFalseContractIsApprovedStatusWithoutSearchText()
        {
            var contractIsNotApproved = "false";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenContractIsApprovedIsProvided(contractIsNotApproved))
                .Then(t => _steps.ThenAssetsWithContractApprovalStatusShouldBeIncluded(contractIsNotApproved, 11))
                .BDDfy();
        }
        [Fact]
        public void ServiceFiltersTrueContractIsActiveStatusWithoutSearchText()
        {
            var contractIsActive = "true";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenContractIsActiveIsProvided(contractIsActive))
                .Then(t => _steps.ThenAssetsWithProvidedContractStatusShouldBeIncluded(contractIsActive, 12))
                .BDDfy();
        }
        [Fact]
        public void ServiceFiltersAssetContractChargesSubtypeWithoutSearchText()
        {
            var chargesSubtype = "rate";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenAssetContractSubtypeIsProvided(chargesSubtype))
                .Then(t => _steps.ThenAssetsWhoseContractHasProvidedChargesSubytpeAreReturned(chargesSubtype, 1))
                .BDDfy();
        }
        [Fact]
        public void ServiceFiltersFalseContractIsActiveStatusWithoutSearchText()
        {
            var contractIsNotActive = "false";
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenContractIsActiveIsProvided(contractIsNotActive))
                .Then(t => _steps.ThenAssetsWithProvidedContractStatusShouldBeIncluded(contractIsNotActive, 1))
                .BDDfy();
        }
        [Fact]
        public void ServiceReturnsAllAssetsWhenNoParameterIsProvided()
        {
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenNoParameterIsProvided())
                .Then(t => _steps.ThenAllAssetsAreReturned(12))
                .BDDfy();
        }
        [Fact]
        public void ServiceReturnsTemporaryAccomodationResultsWhenisTemporaryAccommodationTrue()
        {
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenIsTemporaryAccomodation("true"))
                .Then(t => _steps.ThenOnlyTemporaryAccomodationResultsShouldBeIncluded())
                .BDDfy();
        }
        [Fact]
        public void ServiceReturnsTemporaryAccomodationResultAddressWhereWildstarDoubleMatch()
        // If a search is made for an address string with more than two values in the string, it should only return a match where both wildstar values are found. 
        {
            this.Given(g => _assetsFixture.GivenAnAssetIndexExists())
                .When(w => _steps.WhenIsTemporaryAccomodationIsTrueAndSearchText("19 buckland"))
                .Then(t => _steps.ThenThatTemporaryAccomodationAddressShouldBeTheFirstResult("19 Buckland Court St Johns Estate"))
                .BDDfy();
        }
    }
}
