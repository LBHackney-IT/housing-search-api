using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Gateways
{
    public class SearchAssetsGateway : ISearchAssetsGateway
    {
        private readonly ISearchElasticSearchHelper<GetAssetListRequest, QueryableAsset> _elasticSearchHelper;

        public SearchAssetsGateway(ISearchElasticSearchHelper<GetAssetListRequest, QueryableAsset> elasticSearchHelper)
        {
            _elasticSearchHelper = elasticSearchHelper;
        }
        // TODO: 1 Return when last commit
        //[LogCall]
        public async Task<GetAssetListResponse> GetListOfAssets(GetAssetListRequest query)
        {
            var searchResponse = await _elasticSearchHelper.Search(query).ConfigureAwait(false);

            var assetListResponse = new GetAssetListResponse();

            assetListResponse.Assets.AddRange(searchResponse.Documents.Select(queryableAsset => queryableAsset.Create()));

            assetListResponse.SetTotal(searchResponse.Total);

            return assetListResponse;
        }
    }
}
