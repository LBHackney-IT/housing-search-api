using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps;
using TestStack.BDDfy;
using Xunit;

namespace HousingSearchApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "The Tenure Search All Endpoint to return results",
        SoThat = "it is possible to search for tenures")]
    [Collection("ElasticSearch collection")]
    public class GetAllTenuresStories
    {
        private readonly MockWebApplicationFactory<Startup> _factory;
        private readonly TenureFixture _tenureFixture;
        private readonly GetTenureAllSteps _steps;

        public GetAllTenuresStories(MockWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            var httpClient = factory.CreateClient();
            var elasticClient = factory.ElasticSearchClient;

            _steps = new GetTenureAllSteps(httpClient);
            _tenureFixture = new TenureFixture(elasticClient, httpClient);
        }

        //if search text and uprn are empty TenureQueryGenerator throws an exception that results in bad rquest
        [Fact]
        public void ServiceReturnsBadRequestResult()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenTenuresAllRequestDoesNotContainSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBeBadRequestResult(default))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsOkResult()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenTenuresAllRequestContainSearchString())
                .Then(t => _steps.ThenTheLastRequestShouldBe200())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsCorrectPageSize()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenAPageSizeIsProvidedForTenuresAll(5))
                .Then(t => _steps.ThenTheReturningAllTenureResultsShouldBeOfThatSize(5))
                .BDDfy();
        }

        //deserializing to APIResponse here  
        [Fact]
        public void ServiceReturnsMostRelevantResultFirst()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenSimilarTenures("FirstEntry", "SecondEntry", "ThirdEntry"))
                .When(w => _steps.WhenSearchingForASpecificTenureUsingTenuresAll("FirstEntry", "SecondEntry", "ThirdEntry"))
                .Then(t => _steps.ThenTheFirstOfTheReturningTenureAllResultsShouldBeTheMostRelevantOne("FirstEntry", "SecondEntry", "ThirdEntry"))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnSpecificTenureByUprn()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenATenureWithSpecificUprn("12345678"))
                .When(w => _steps.WhenTenuresAllRequestContainsUprn("12345678"))
                .Then(t => _steps.ThenTheReturningTenureAllResultShouldBeTheSpecificTenure("12345678", 1))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsAllTaTenures()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenTaTenuresExist(5))
                .When(w => _steps.WhenSearchingForAllTaTenuresUsingTenuresAll())
                .Then(t => _steps.ThenTheReturningTenureAllResultsShouldIncludeAllTaTenures(5))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsFilteredByBookingStatusTaTenures()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenTaTenuresExist(5))
                .Given(g => _tenureFixture.GivenSimilarTaTenuresExist("ACC", "John Doe"))
                .When(w => _steps.WhenSearchingForTaTenuresUsingTenuresAllWithABookingStatusAndNoSearchText("ACC"))
                .Then(t => _steps.ThenTheReturningTenureAllResultsShouldBeTheFilteredTaTenures("ACC", 2))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsTaTenuresWhenSearchedByNameButNotFilteredByBookingStatus()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenTaTenuresExist(5))
                .Given(g => _tenureFixture.GivenSimilarTaTenuresExist("ACC", "John Doe"))
                .When(w => _steps.WhenSearchingForASpecificTaTenureByTenantFullNameUsingTenuresAll("John Doe"))
                .Then(t => _steps.ThenTheReturningTenureAllResultsShouldBeTheSearchedTaTenures("John Doe", 2))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsSpecificTenuresWhenSearchedByNameAndFilteredByBookingStatus()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenTaTenuresExist(5))
                .Given(g => _tenureFixture.GivenSimilarTaTenuresExist("ACC", "John Doe"))
                .When(w => _steps.WhenSearchingForASpecificTaTenureByBookingStatusAndTenantFullNameUsingTenuresAll("ACC", "John Doe"))
                .Then(t => _steps.ThenTheReturningTenureALlResultShouldHaveTheSpecificTaTenure("ACC", "John Doe", 1))
                .BDDfy();

        }

        [Fact]
        public void ServiceReturnsLatestTenuresFirstWhenSortingByTenureStartDateIsSetToDesc()
        {
            var isDesc = true;
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenTenuresWithDifferentStartDatesExist())
                .When(w => _steps.WhenSearchingForTenuresWithSortingByTenureStartDateUsingTenuresAll(isDesc))
                .Then(t => _steps.ThenTheReturningTenureAllResultsShouldBeSortedByDescendingTenureStartDate())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsLatestTenuresLastWhenSortingByTenureStartDateIsSetToAsc()
        {
            var isDesc = false;
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenTenuresWithDifferentStartDatesExist())
                .When(w => _steps.WhenSearchingForTenuresWithSortingByTenureStartDateUsingTenuresAll(isDesc))
                .Then(t => _steps.ThenTheReturningTenureAllResultsShouldBeSortedByAscendingTenureStartDate())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsTotalInTheResponse()
        {
            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .When(w => _steps.WhenSearchingForTenuresUsingTenuresAll())
                .Then(t => _steps.ThenTheReturningTenureAllResultsContainTotalValueOtherThanZero())
                .BDDfy();
        }

        //last hit id tests require items with known IDs, start dates and content
        [Fact]
        public void ServiceReturnsFirstSetOfResultsWhenLastHitIdIsNotSetInTheRequestWhilePageSizeAndSortingByTenureStartDateToDescendingAreSet()
        {

            var oldestRecord = "caec45d1-8034-419b-bc55-ff491079b628";
            var middleRecord = "ecbf9d02-3b1c-4e9f-92ab-0ddb74b80b27";
            var latestRecord = "a04d59de-1f2c-4cca-ab20-cb501347e226";
            var isDesc = true;
            var pageSize = 2;

            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenTenuresWithSpecificContentExist(oldestRecord, middleRecord, latestRecord))
                .When(w => _steps.WhenSearchingForTenuresWithSearchTextSortingByTenureStartDateDescAndSettingPageSizeUsingTenuresAll(pageSize, isDesc))
                .Then(t => _steps.ThenTheReturningTenureAllResultsContainCorrectRecordsInCorrectOrder(latestRecord, middleRecord, pageSize))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsFirstSetOfResultsWhenLastHitIdIsInTheRequestWhilePageSizeAndSortingByTenureStartDateToDescendingAreSet()
        {
            var oldestRecord = "caec45d1-8034-419b-bc55-ff491079b690";
            var middleRecord = "ecbf9d02-3b1c-4e9f-92ab-0ddb74b80b46";
            var latestRecord = "a04d59de-1f2c-4cca-ab20-cb501347e2bb";
            var middleRecordTenureStartDateInMillisecondsSinceEpoch = "1716822489000"; //"2024-05-27T15:08:09Z", set in the firstItemOnTheSecondSet 
            var isDesc = true;
            var pageSize = 2;

            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenTenuresWithSpecificContentExist(oldestRecord, middleRecord, latestRecord))
                .When(w => _steps.WhenSearchingForTenuresWithSearchTextSortingByTenureStartDateDescAndSettingPageSizeAndLastHitIdAndLastHitTenureStartDateUsingTenuresAll(pageSize, middleRecord, middleRecordTenureStartDateInMillisecondsSinceEpoch, isDesc))
                .Then(t => _steps.ThenReturningTenureAllResultsContainSingleCorrectTenure(oldestRecord))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsFirstSetOfResultsWhenLastHitIdIsNotSetInTheRequestWhilePageSizeAndSortingByTenureStartDateToAscendingAreSet()
        {
            var oldestRecord = "caec45d1-8034-419b-bc55-ff491079b628";
            var middleRecord = "ecbf9d02-3b1c-4e9f-92ab-0ddb74b80b27";
            var latestRecord = "a04d59de-1f2c-4cca-ab20-cb501347e226";
            var isDesc = false;
            var pageSize = 2;

            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenTenuresWithSpecificContentExist(oldestRecord, middleRecord, latestRecord))
                .When(w => _steps.WhenSearchingForTenuresWithSearchTextSortingByTenureStartDateDescAndSettingPageSizeUsingTenuresAll(pageSize, isDesc))
                .Then(t => _steps.ThenTheReturningTenureAllResultsContainCorrectRecordsInCorrectOrder(oldestRecord, middleRecord, pageSize))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsFirstSetOfResultsWhenLastHitIdIsInTheRequestWhilePageSizeAndSortingByTenureStartDateToAscendingAreSet()
        {
            var oldestRecord = "caec45d1-8034-419b-bc55-ff491079b690";
            var middleRecord = "ecbf9d02-3b1c-4e9f-92ab-0ddb74b80b46";
            var latestRecord = "a04d59de-1f2c-4cca-ab20-cb501347e2bb";
            var middleRecordTenureStartDateInMillisecondsSinceEpoch = "1716822489000";
            var isDesc = false;
            var pageSize = 2;

            this.Given(g => _tenureFixture.GivenATenureIndexExists())
                .Given(g => _tenureFixture.GivenTenuresWithSpecificContentExist(oldestRecord, middleRecord, latestRecord))
                .When(w => _steps.WhenSearchingForTenuresWithSearchTextSortingByTenureStartDateDescAndSettingPageSizeAndLastHitIdAndLastHitTenureStartDateUsingTenuresAll(pageSize, middleRecord, middleRecordTenureStartDateInMillisecondsSinceEpoch, isDesc))
                .Then(t => _steps.ThenReturningTenureAllResultsContainSingleCorrectTenure(latestRecord))
                .BDDfy();
        }
    }
}

