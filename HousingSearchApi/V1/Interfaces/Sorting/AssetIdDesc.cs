using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class AssetIdDesc : ISort<QueryableAsset>
{
    private readonly ISort<QueryableAsset> _parentSort;
 
    public AssetIdDesc(ISort<QueryableAsset> parentSort)
    {
        _parentSort = parentSort;
    }
   
    public SortDescriptor<QueryableAsset> GetSortDescriptor(SortDescriptor<QueryableAsset> descriptor)
    {
        if (_parentSort != null)
        {
            descriptor = _parentSort.GetSortDescriptor(descriptor);
        }
       
        return descriptor.Descending(f => f.Id.Suffix("keyword"));
    }
}

}
