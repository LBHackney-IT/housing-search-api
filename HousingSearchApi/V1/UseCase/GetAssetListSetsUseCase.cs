using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;
using HousingSearchApi.V1.Gateways.Interfaces;

namespace HousingSearchApi.V1.UseCase
{
    public class GetAssetListSetsUseCase : IGetAssetListSetsUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public GetAssetListSetsUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        public async Task<GetAllAssetListResponse> ExecuteAsync(GetAllAssetListRequest query)
        {
            return await _searchGateway.GetListOfAssetsSets(query).ConfigureAwait(false);
        }
    }
}
