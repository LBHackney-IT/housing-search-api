using FluentAssertions;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps.Base;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps
{
    public class GetStaffsSteps : BaseSteps
    {
        public GetStaffsSteps(HttpClient httpClient) : base(httpClient)
        {
        }
        public async Task WhenRequestDoesNotContainSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/staffs", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenRequestContainsSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/staffs?searchText=abc", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenSearchingByEmailAddress(string emailaddress)
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"api/v1/search/staffs?searchText=%20{emailaddress}", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenAPageSizeIsProvided(int pageSize)
        {
            var route = new Uri($"api/v1/search/staffs?searchText={StaffFixture.Staffs.Last().EmailAddress}&pageSize={pageSize}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task ThenTheReturningResultsShouldBeOfThatSize(int pageSize)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetStaffListResponse>>(resultBody, _jsonOptions);

            result.Results.Staffs.Count.Should().Be(pageSize);
        }

        public async Task ThenTheFirstResultShouldBeAnExactMatchOfEmailAddress(string emailAddress)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetStaffListResponse>>(resultBody, _jsonOptions);

            result.Results.Staffs.First().EmailAddress.Should().Be(emailAddress);
        }


    }
}
