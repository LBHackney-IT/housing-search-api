using FluentAssertions;
using Hackney.Shared.Processes.Domain.Constants;
using HousingSearchApi.Tests.V1.E2ETests.Fixtures;
using HousingSearchApi.Tests.V1.E2ETests.Steps.Base;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps
{
    public class GetProcessesSteps : BaseSteps
    {
        public GetProcessesSteps(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task WhenRequestDoesNotContainSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri("api/v1/search/processes", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenRequestContainsSearchString()
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"api/v1/search/processes?searchText={ProcessFixture._patchAssignment.PatchId}&targetId={ProcessFixture.Processes[0].TargetId}", UriKind.Relative)).ConfigureAwait(false);
        }


        public async Task WhenAPageSizeIsProvided(int pageSize)
        {
            var route = new Uri($"api/v1/search/processes?searchText={ProcessFixture._patchAssignment.PatchId}&pageSize={pageSize}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenTargetTypeAndTargetIdAreProvided(string targetType, string targetId)
        {
            var route = new Uri($"api/v1/search/processes?searchText={ProcessFixture._patchAssignment.PatchId}&targetId={targetId}&targetType={targetType}&pageSize={5}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenProcessNameIsProvided(string processName)
        {
            var route = new Uri($"api/v1/search/processes?searchText={ProcessFixture._patchAssignment.PatchId}&processName={processName}&pageSize={5}",
                            UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenProcessStatusIsProvided(bool isOpen)
        {
            var route = new Uri($"api/v1/search/processes?searchText={ProcessFixture._patchAssignment.PatchId}&isOpen={isOpen}&pageSize={5}",
                            UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }


        public async Task WhenTargetTypeIsProvided(string targetType)
        {
            var route = new Uri($"api/v1/search/processes?searchText={ProcessFixture._patchAssignment.PatchId}&targetType={targetType}&pageSize={5}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task ThenTheReturningResultsShouldBeOfThatSize(int pageSize)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetProcessListResponse>>(resultBody, _jsonOptions);

            result.Results.Processes.Count.Should().Be(pageSize);
        }

        public async Task ThenOnlyTheseTargetTypeAndTargetIdShouldBeIncluded(string allowedTargetType, string allowedTargetId)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetProcessListResponse>>(resultBody, _jsonOptions);


            result.Results.Processes.All(x => x.TargetType == allowedTargetType && x.TargetId == allowedTargetId);
        }

        public async Task ThenOnlyTheProcessNameShouldBeIncluded(string allowedProcessName)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetProcessListResponse>>(resultBody, _jsonOptions);


            result.Results.Processes.All(x => x.ProcessName.ToString() == allowedProcessName);
        }

        public async Task ThenOnlyTheProcessStatusShouldBeIncluded(bool isOpen)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetProcessListResponse>>(resultBody, _jsonOptions);

            result.Results.Processes.Should().NotBeEmpty();

            var closedStates = new List<string>() { SharedStates.ProcessCancelled, SharedStates.ProcessClosed, SharedStates.ProcessCompleted };

            var validateClosedStates = result.Results.Processes.All(x => x.State == closedStates.FirstOrDefault());

            var validateProcessState = isOpen ? validateClosedStates : !validateClosedStates;
        }

    }
}
