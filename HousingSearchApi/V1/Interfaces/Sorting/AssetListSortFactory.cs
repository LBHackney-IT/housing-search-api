using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class AssetListSortFactory : IListSortFactory<GetAssetListRequest, QueryableAsset>
    {
        public SortDescriptor<QueryableAsset> DynamicSort(SortDescriptor<QueryableAsset> sortDescriptor, GetAssetListRequest request)
        {
            var sortBy = request.SortBy?.ToLower();

            switch (sortBy)
            {
                case "assetname":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.AssetName);
                    break;

                case "assetid":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.AssetId);
                    break;

                case "postCode":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.QueryableAssetAddress.PostCode);
                    break;

                case "address":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.QueryableAssetAddress.AddressLine1);
                    break;

                case "totalbalance":
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.TotalBalance);
                    break;

                default:
                    sortDescriptor.SetSortOrder(request.IsDesc, x => x.AssetId);
                    break;
            }

            return sortDescriptor;
        }
    }
}
