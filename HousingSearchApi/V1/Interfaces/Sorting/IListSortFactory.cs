using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public interface IListSortFactory<TRequest, TQueryable>
        where TRequest : class
        where TQueryable : class
    {
        SortDescriptor<TQueryable> DynamicSort(SortDescriptor<TQueryable> sortDescriptor, TRequest request);
    }
}
