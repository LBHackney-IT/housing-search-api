using System.Threading.Tasks;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;

namespace HousingSearchApi.V1.UseCase
{
    public class GetAssetListUseCase : IGetAssetListUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public GetAssetListUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        [LogCall]
        public async Task<GetAssetListResponse> ExecuteAsync(GetAssetListRequest housingSearchRequest)
        {
            return await _searchGateway.GetListOfAssets(housingSearchRequest).ConfigureAwait(false);
        }
    }
}
