using FluentAssertions;
using HousingSearchApi.Tests.V1.E2ETests.Steps.Base;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps
{
    public class GetTenureAllSteps : BaseSteps
    {
        public GetTenureAllSteps(HttpClient httpclient) : base(httpclient)
        {
        }

        // when
        public async Task WhenTenuresAllRequestDoesNotContainSearchString()
        {
            var uri = new Uri("api/v1/search/tenures/all", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        public async Task WhenTenuresAllRequestContainSearchString()
        {
            var uri = new Uri("api/v1/search/tenures/all?searchText=abc", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }
        public async Task WhenAPageSizeIsProvidedForTenuresAll(int pageSize)
        {
            var uri = new Uri($"api/v1/search/tenures/all?searchText=abc&&pageSize={pageSize}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }
        public async Task WhenSearchingForASpecificTenureUsingTenuresAll(string paymentReference, string fullAddress, string fullName)
        {
            var uri = new Uri($"api/v1/search/tenures/all?searchText={paymentReference}%20{fullAddress}%20{fullName}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }
        public async Task WhenTenuresAllRequestContainsUprn(string uprn)
        {
            var uri = new Uri($"api/v1/search/tenures/all?uprn={uprn}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }
        public async Task WhenSearchingForAllTaTenuresUsingTenuresAll()
        {
            var uri = new Uri("api/v1/search/tenures/all?searchText=\"\"&isTemporaryAccommodation=true", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }
        public async Task WhenSearchingForTaTenuresUsingTenuresAllWithABookingStatusAndNoSearchText(string bookingStatus)
        {
            var uri = new Uri($"api/v1/search/tenures/all?bookingStatus={bookingStatus}&searchText=\"\"&isTemporaryAccommodation=true", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }
        public async Task WhenSearchingForASpecificTaTenureByTenantFullNameUsingTenuresAll(string fullName)
        {
            var uri = new Uri($"api/v1/search/tenures/all?searchText={fullName}&isTemporaryAccommodation=true", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }
        public async Task WhenSearchingForASpecificTaTenureByBookingStatusAndTenantFullNameUsingTenuresAll(string bookingStatus, string fullName)
        {
            var uri = new Uri($"api/v1/search/tenures/all?bookingStatus={bookingStatus}&searchText={fullName}&isTemporaryAccommodation=true", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }
        public async Task WhenSearchingForTenuresWithSortingByTenureStartDateUsingTenuresAll(bool isDesc)
        {
            var uri = new Uri($"api/v1/search/tenures/all?searchText=%22%22&sortBy=tenureStartDate&isDesc={isDesc}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        public async Task WhenSearchingForTenuresUsingTenuresAll()
        {
            var uri = new Uri("api/v1/search/tenures/all?searchText=%22%22", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        public async Task WhenSearchingForTenuresWithSearchTextSortingByTenureStartDateDescAndSettingPageSizeUsingTenuresAll(int pageSize, bool isDesc)
        {
            var uri = new Uri($"api/v1/search/tenures/all?searchText=veryspecificpaymentreferencefortestinglasthitid&sortBy=tenureStartDate&isDesc={isDesc}&pageSize={pageSize}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        public async Task WhenSearchingForTenuresWithSearchTextSortingByTenureStartDateDescAndSettingPageSizeAndLastHitIdAndLastHitTenureStartDateUsingTenuresAll(int pageSize, string lastHitId, string lastHitTenureStartDate, bool isDesc)
        {
            var uri = new Uri($"api/v1/search/tenures/all?searchText=veryspecificpaymentreferencefortestinglasthitid&sortBy=tenureStartDate&isDesc={isDesc}&pageSize={pageSize}&lastHitId={lastHitId}&lastHitTenureStartDate={lastHitTenureStartDate}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        // then
        public async Task ThenTheReturningAllTenureResultsShouldBeOfThatSize(int pageSize)
        {
            var result = await DeserializeResults();

            result.Results.Tenures.Count.Should().Be(pageSize);
        }

        public async Task ThenTheFirstOfTheReturningTenureAllResultsShouldBeTheMostRelevantOne(string paymentReference, string fullAddress, string fullName)
        {
            var result = await DeserializeResults();

            result.Results.Tenures.First().PaymentReference.Should().Be(paymentReference);
            result.Results.Tenures.First().TenuredAsset.FullAddress.Should().Be(fullAddress);
            result.Results.Tenures.First().HouseholdMembers.First().FullName.Should().Be(fullName);
        }

        public async Task ThenTheReturningTenureAllResultShouldBeTheSpecificTenure(string uprn, int tenureCount)
        {
            var result = await DeserializeResults();

            result.Results.Tenures.Count.Should().Be(tenureCount);
            result.Results.Tenures.First().TenuredAsset.Uprn.Should().Be(uprn);
        }

        public async Task ThenTheReturningTenureAllResultsShouldIncludeAllTaTenures(int amountOfTaTenures)
        {
            var result = await DeserializeResults();

            var tenures = result.Results.Tenures;
            tenures.Count.Should().Be(amountOfTaTenures);
            foreach (var tenure in tenures)
            {
                tenure.TenuredAsset.IsTemporaryAccommodation.Should().Be(true);
            }
        }

        public async Task ThenTheReturningTenureAllResultsShouldBeTheFilteredTaTenures(string bookingStatus, int tenureCount)
        {
            var result = await DeserializeResults();

            var tenures = result.Results.Tenures;
            tenures.Count.Should().Be(tenureCount);
            foreach (var tenure in tenures)
            {
                tenure.TempAccommodationInfo.BookingStatus.Should().Be(bookingStatus);
            }
        }

        public async Task ThenTheReturningTenureAllResultsShouldBeTheSearchedTaTenures(string fullName, int tenureCount)
        {
            var result = await DeserializeResults();

            var tenures = result.Results.Tenures;
            tenures.Count.Should().Be(tenureCount);
            foreach (var tenure in tenures)
            {
                tenure.HouseholdMembers.First().FullName.Should().Be(fullName);
            }
        }

        public async Task ThenTheReturningTenureALlResultShouldHaveTheSpecificTaTenure(string bookingStatus, string fullName, int tenureCount)
        {
            var result = await DeserializeResults();

            var tenures = result.Results.Tenures;
            tenures.Count.Should().Be(tenureCount);
            tenures.First().TempAccommodationInfo.BookingStatus.Should().Be(bookingStatus);
            tenures.First().HouseholdMembers.First().FullName.Should().Be(fullName);
        }

        public async Task ThenTheReturningTenureAllResultsShouldBeSortedByDescendingTenureStartDate()
        {
            var result = await DeserializeResults();

            var tenures = result.Results.Tenures;

            CollectionAssert.AreEqual(tenures.OrderByDescending(x => x.StartOfTenureDate), tenures);
        }

        public async Task ThenTheReturningTenureAllResultsShouldBeSortedByAscendingTenureStartDate()
        {
            var result = await DeserializeResults();

            var tenures = result.Results.Tenures;

            CollectionAssert.AreEqual(tenures.OrderBy(x => x.StartOfTenureDate), tenures);
        }

        public async Task ThenTheReturningTenureAllResultsContainTotalValueOtherThanZero()
        {
            var result = await DeserializeResults();

            result.Total.Should().NotBe(0);
        }

        public async Task ThenTheReturningTenureAllResultsContainCorrectRecordsInCorrectOrder(string firstId, string lastId, int expectedCount)
        {
            var result = await DeserializeResults();

            //see test setup for specific record
            result.Results.Tenures.Count.Should().Be(expectedCount);
            result.Results.Tenures.First().Id.Should().Be(firstId);
            result.Results.Tenures.Last().Id.Should().Be(lastId);
        }

        public async Task ThenReturningTenureAllResultsContainSingleCorrectTenure(string idToMatch)
        {
            var result = await DeserializeResults();

            result.Results.Tenures.Count.Should().Be(1);
            result.Results.Tenures.First().Id.Should().Be(idToMatch);
        }

        private async Task<APIAllResponse<GetAllTenureListResponse>> DeserializeResults()
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIAllResponse<GetAllTenureListResponse>>(resultBody, _jsonOptions);

            return result;
        }
    }
}
