using System.Threading.Tasks;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;

namespace HousingSearchApi.V1.UseCase
{
    public class GetAssetRelationshipsUseCase : IGetAssetRelationshipsUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public GetAssetRelationshipsUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        [LogCall]
        public async Task<GetAssetRelationshipsResponse> ExecuteAsync(GetAssetRelationshipsRequest getAssetRelationshipsRequest)
        {
            var response = new GetAssetRelationshipsResponse
            {
                ChildAssets = await _searchGateway.GetChildAssets(getAssetRelationshipsRequest).ConfigureAwait(false)
            };

            return response;
        }
    }
}
