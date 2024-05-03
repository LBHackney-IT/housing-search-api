using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps.Base;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps
{
    public class GetTenureSteps : BaseSteps
    {
        public GetTenureSteps(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task WhenRequestDoesNotContainSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/tenures", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenRequestContainsSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/tenures?searchText=abc", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenRequestContainsUprn(string uprn)
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"api/v1/search/tenures?uprn={uprn}", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenSearchingForAllTaTenures()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/tenures?searchText=\"\"&isTemporaryAccommodation=true", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenSearchingForTaTenuresWithABookingStatusAndNoSearchText(string bookingStatus)
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"api/v1/search/tenures?bookingStatus={bookingStatus}&searchText=\"\"&isTemporaryAccommodation=true", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenSearchingForASpecificTaTenureByBookingStatusAndTenantFullName(string bookingStatus, string fullName)
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"api/v1/search/tenures?bookingStatus={bookingStatus}&searchText={fullName}&isTemporaryAccommodation=true", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenSearchingForASpecificTaTenureByTenantFullName(string fullName)
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"api/v1/search/tenures?searchText={fullName}&isTemporaryAccommodation=true", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenSearchingForASpecificTenure(string paymentReference, string fullAddress, string fullName)
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"api/v1/search/tenures?searchText={paymentReference}%20{fullAddress}%20{fullName}", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenAPageSizeIsProvided(int pageSize)
        {
            var route = new Uri($"api/v1/search/tenures?searchText={TenureFixture.Alphabet.Last()}&pageSize={pageSize}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task ThenTheReturningResultsShouldBeOfThatSize(int pageSize)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetTenureListResponse>>(resultBody, _jsonOptions);

            result.Results.Tenures.Count.Should().Be(pageSize);
        }

        public async Task ThenTheFirstOfTheReturningResultsShouldBeTheMostRelevantOne(string paymentReference, string fullAddress, string fullName)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetTenureListResponse>>(resultBody, _jsonOptions);

            result.Results.Tenures.First().PaymentReference.Should().Be(paymentReference);
            result.Results.Tenures.First().TenuredAsset.FullAddress.Should().Be(fullAddress);
            result.Results.Tenures.First().HouseholdMembers.First().FullName.Should().Be(fullName);
        }

        public async Task ThenTheReturningResultShouldBeTheSpecificTenure(string uprn, int tenureCount)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetTenureListResponse>>(resultBody, _jsonOptions);

            result.Results.Tenures.Count.Should().Be(tenureCount);
            result.Results.Tenures.First().TenuredAsset.Uprn.Should().Be(uprn);
        }

        public async Task ThenTheReturningResultsShouldIncludeAllTaTenures(int amountOfTaTenures)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetTenureListResponse>>(resultBody, _jsonOptions);


            var tenures = result.Results.Tenures;
            tenures.Count.Should().Be(amountOfTaTenures);
            foreach (var tenure in tenures)
            {
                tenure.TenuredAsset.IsTemporaryAccommodation.Should().Be(true);
            }
        }

        public async Task ThenTheReturningResultsShouldBeTheFilteredTaTenures(string bookingStatus, int tenureCount)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetTenureListResponse>>(resultBody, _jsonOptions);

            var tenures = result.Results.Tenures;
            tenures.Count.Should().Be(tenureCount);
            foreach (var tenure in tenures)
            {
                tenure.TempAccommodationInfo.BookingStatus.Should().Be(bookingStatus);
            }
        }

        public async Task ThenTheReturningResultsShouldBeTheSearchedTaTenures(string fullName, int tenureCount)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetTenureListResponse>>(resultBody, _jsonOptions);

            var tenures = result.Results.Tenures;
            tenures.Count.Should().Be(tenureCount);
            foreach (var tenure in tenures)
            {
                tenure.HouseholdMembers.First().FullName.Should().Be(fullName);
            }
        }

        public async Task ThenTheReturningResultShouldHaveTheSpecificTaTenure(string bookingStatus, string fullName, int tenureCount)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetTenureListResponse>>(resultBody, _jsonOptions);

            var tenures = result.Results.Tenures;
            tenures.Count.Should().Be(tenureCount);
            tenures.First().TempAccommodationInfo.BookingStatus.Should().Be(bookingStatus);
            tenures.First().HouseholdMembers.First().FullName.Should().Be(fullName);
        }


    }
}
