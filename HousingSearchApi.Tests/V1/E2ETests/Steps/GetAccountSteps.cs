using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps.Base;
using HousingSearchApi.Tests.V1.E2ETests.Steps.ResponseModels;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps
{
    public class GetAccountSteps : BaseSteps
    {
        public GetAccountSteps(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task WhenRequestDoesNotContainSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/accounts", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenRequestContainsSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/accounts?searchText=%20abc", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenAPageSizeIsProvided(int pageSize)
        {
            var route = new Uri($"api/v1/search/accounts?searchText={AccountFixture.AccountSearchStubs.Last().PaymentReference}&pageSize={pageSize}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenTargetIdsAreProvided(Guid targetId)
        {
            var route = new Uri($"api/v1/search/accounts?targetId={targetId}&pageSize={5}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenAnExactMatchExists(string address)
        {
            var route = new Uri($"api/v1/search/accounts?searchText={address}&pageSize={5}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public void ThenTheLastRequestShouldBeBadRequestResult()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public async Task ThenTheReturningResultsShouldBeOfThatSize(int pageSize)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<AccountListDTO>>(resultBody, _jsonOptions);

            result?.Results.Accounts.Count.Should().Be(pageSize);
        }

        public async Task ThenOnlyTheseAccountTargetIdsShouldBeIncluded(Guid allowedTargetId)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<AccountListDTO>>(resultBody, _jsonOptions);

            result?.Results.Accounts.ForEach(x => x.TargetId.Should().Be(allowedTargetId));
        }

        public async Task ThenThatAddressShouldBeTheFirstResult(string address)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<AccountListDTO>>(resultBody, _jsonOptions);

            result?.Results.Accounts.First().Tenure.FullAddress.Should().Be(address);
        }
    }
}
