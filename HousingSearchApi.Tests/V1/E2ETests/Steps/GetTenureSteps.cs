using System;
using System.Linq;
using System.Net;
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

        public void ThenTheLastRequestShouldBeBadRequestResult()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public void ThenTheLastRequestShouldBe200()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);
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
    }
}
