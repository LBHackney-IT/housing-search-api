using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class AssetListSortFactory : IListSortFactory<GetAssetListRequest, QueryableAsset>
    {
        public SortDescriptor<QueryableAsset> DynamicSort(SortDescriptor<QueryableAsset> f, GetAssetListRequest request)
        {
            var sortBy = request.SortBy.ToLower();

            // TODO: Add other fields
            switch (sortBy)
            {
                case "assetname":
                    f.SetSortOrder(request.IsDesc, x => x.AssetName);
                    break;

                default:
                    f.SetSortOrder(request.IsDesc, x => x.AssetName);
                    break;
            }

            return f;
        }
    }
}
