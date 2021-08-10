using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.UseCase
{
    public class GetAssetListUseCase : IGetAssetListUseCase
    {
        private readonly ISearchAssetsGateway _searchAssetsGateway;

        public GetAssetListUseCase(ISearchAssetsGateway searchPersonsGateway)
        {
            _searchAssetsGateway = searchPersonsGateway;
        }
        [LogCall]
        public async Task<GetAssetListResponse> ExecuteAsync(GetAssetListRequest getAssetListRequest)
        {
            var response = await _searchAssetsGateway.GetListOfAssets(getAssetListRequest).ConfigureAwait(false);

            return response;
        }
    }
}
