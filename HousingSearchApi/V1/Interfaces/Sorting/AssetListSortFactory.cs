using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class AssetListSortFactory : IListSortFactory<GetAssetListRequest, QueryableAsset>
    {
        public SortDescriptor<QueryableAsset> DynamicSort(SortDescriptor<QueryableAsset> f, GetAssetListRequest request)
        {
            var sortBy = request.SortBy.ToLower();

            // TODO: Add other fields
            switch (request.SortBy)
            {
                case "assetname":
                    if (!request.IsDesc)
                    {
                        f.Ascending(x => x.AssetName);
                    }
                    else
                    {
                        f.Descending(x => x.AssetName);
                    }
                    break;

                default:
                    if (!request.IsDesc)
                    {
                        f.Ascending(x => x.AssetName);
                    }
                    else
                    {
                        f.Descending(x => x.AssetName);
                    }
                    break;
            }

            return f;
        }
    }
}
