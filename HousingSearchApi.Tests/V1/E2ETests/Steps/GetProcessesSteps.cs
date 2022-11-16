using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Process;
using Hackney.Shared.Processes.Domain;
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

        public async Task WhenRequestContainsSearchString(string patchId)
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"api/v1/search/processes?searchText={patchId}", UriKind.Relative)).ConfigureAwait(false);
        }

        public async Task WhenRequestContainsBothTargetIdAndTargetType(string targetId, string targetType)
        {
            _lastResponse = await _httpClient.GetAsync(new Uri($"api/v1/search/processes?targetId={targetId}&targetType={targetType}", UriKind.Relative)).ConfigureAwait(false);
        }


        public async Task WhenAPageSizeIsProvided(int pageSize)
        {
            var route = new Uri($"api/v1/search/processes?searchText={ProcessFixture.PatchAssignment.PatchId}&pageSize={pageSize}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenTargetTypeAndTargetIdAreProvided(string targetType, string targetId)
        {
            var route = new Uri($"api/v1/search/processes?searchText={ProcessFixture.PatchAssignment.PatchId}&targetId={targetId}&targetType={targetType}&pageSize={5}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenProcessNameIsProvided(string processName)
        {
            var route = new Uri($"api/v1/search/processes?searchText={ProcessFixture.PatchAssignment.PatchId}&processName={processName}&pageSize={5}",
                            UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenProcessStatusIsProvided(bool isOpen)
        {
            var route = new Uri($"api/v1/search/processes?searchText={ProcessFixture.PatchAssignment.PatchId}&isOpen={isOpen}&pageSize={5}",
                            UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }


        public async Task WhenTargetTypeIsProvided(string targetType)
        {
            var route = new Uri($"api/v1/search/processes?searchText={ProcessFixture.PatchAssignment.PatchId}&targetType={targetType}&pageSize={5}",
                UriKind.Relative);

            _lastResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);
        }

        public async Task WhenARequestIsSortedByAFieldAsc(string fieldName)
        {
            var route = new Uri($"api/v1/search/processes?searchText={ProcessFixture.PatchAssignment.PatchId}&sortBy={fieldName}&isDesc=false",
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

        public async Task ThenTheFirstResultShouldBeAnExactMatchOfPatchId(string patchId)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetProcessListResponse>>(resultBody, _jsonOptions);

            result.Results.Processes.First().PatchAssignment.PatchId.Should().Be(patchId);
        }

        public async Task ThenTheFirstResultShouldBeAnExactMatchOfTargetId(string targetId)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetProcessListResponse>>(resultBody, _jsonOptions);

            result.Results.Processes.First().TargetId.Should().Be(targetId);
        }

        public async Task ThenTheResultShouldBeSortedAsc(string sortBy, Hackney.Shared.HousingSearch.Domain.Process.Process[] processes)
        {
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<APIResponse<GetProcessListResponse>>(resultBody, _jsonOptions);

            var firstProcessIdAfterSort = DetermineFirstProcessId(sortBy, processes);
            result.Results.Processes.First().Id.Should().Be(firstProcessIdAfterSort);
        }

        private string DetermineFirstProcessId(string sortBy, Hackney.Shared.HousingSearch.Domain.Process.Process[] processes)
        {
            switch (sortBy)
            {
                case "name":
                    return processes
                        .OrderBy(x => x.RelatedEntities.Where(x => x.TargetType == TargetType.person.ToString()).Min(x => x.Description))
                        .First()?.Id;
                case "process":
                    return processes.OrderBy(x => x.ProcessName).First()?.Id;
                case "patch":
                    return processes.OrderBy(x => x.PatchAssignment.PatchName).First()?.Id;
                case "state":
                    return processes.OrderBy(x => x.State).First()?.Id;
            }

            return processes.First()?.Id;
        }
    }
}
