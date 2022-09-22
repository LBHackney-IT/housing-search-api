using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.UseCase
{
    public class GetProcessListUseCase : IGetProcessListUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public GetProcessListUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        [LogCall]
        public async Task<GetProcessListResponse> ExecuteAsync(GetProcessListRequest housingSearchRequest)
        {
            return await _searchGateway.GetListOfProcesses(housingSearchRequest).ConfigureAwait(false);
        }
    }
}
