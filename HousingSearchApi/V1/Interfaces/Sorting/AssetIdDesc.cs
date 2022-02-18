using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class AssetIdDesc : ISort<QueryableAsset>
    {
        public SortDescriptor<QueryableAsset> GetSortDescriptor(SortDescriptor<QueryableAsset> descriptor)
        {
            return descriptor
                .Descending(f => f.Id.Suffix("keyword"));
        }
    }
}
