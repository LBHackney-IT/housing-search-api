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
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps
{
    public class GetAssetSteps : BaseSteps
    {
        public GetAssetSteps(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task WhenRequestDoesNotContainSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/assets", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenRequestContainsSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/assets?searchText=%20abc", UriKind.Relative)).ConfigureAwait(false);
        }


        public async Task WhenAPageSizeIsProvided(int pageSize)
        {
            var route = new Uri($"api/v1/search/assets?searchText={AssetFixture.Addresses.Last().FirstLine}&pageSize={pageSize}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }
        public async Task WhenAssetTypesAreProvided(string assetType)
        {
            var route = new Uri($"api/v1/search/assets?searchText={AssetFixture.Addresses.Last()}&assetTypes={assetType}&pageSize={5}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }
        public async Task WhenSearchTextProvidedAsStarStarAndAssetTypeProvided(string assetType)
        {
            var route = new Uri($"api/v1/search/assets/all?searchText=**&assetTypes={assetType}&pageSize={5}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }
        public async Task WhenAnExactMatchExists(string address)
        {
            var route = new Uri($"api/v1/search/assets?searchText={address}&pageSize={5}",
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
            var result = JsonSerializer.Deserialize<APIResponse<GetAssetListResponse>>(resultBody, _jsonOptions);

            result.Results.Assets.Count.Should().Be(pageSize);
        }

        public async Task ThenOnlyTheseAssetTypesShouldBeIncluded(string allowedAssetType)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetAssetListResponse>>(resultBody, _jsonOptions);

            var assets = allowedAssetType.Split(",");

            result.Results.Assets.All(x => x.AssetType == assets[0] || x.AssetType == assets[1]);

        }

        public async Task ThenThatAddressShouldBeTheFirstResult(string address)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetAssetListResponse>>(resultBody, _jsonOptions);

            result.Results.Assets.First().AssetAddress.AddressLine1.Should().Be(address);
        }
    }
}
