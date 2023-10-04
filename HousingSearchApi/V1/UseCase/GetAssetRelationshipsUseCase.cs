using System.Linq;
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

        public async Task<GetAssetRelationshipsResponse> ExecuteAsync(GetAssetRelationshipsRequest request)
        {
            var childAssets = await _searchGateway.GetChildAssets(request).ConfigureAwait(false);

            if (childAssets.Any())
            {
                // Child assets may have matched partially on the GUID
                // Filter to ensure the whole GUID is contained in parentAssetIds
                childAssets = childAssets.Where(x => x.ParentAssetIds.Contains(request.SearchText)).ToList();
            }

            var response = new GetAssetRelationshipsResponse
            {
                ChildAssets = childAssets
            };

            return response;
        }
    }
}
