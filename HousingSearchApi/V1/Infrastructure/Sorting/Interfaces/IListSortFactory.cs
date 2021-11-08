using Nest;

namespace HousingSearchApi.V1.Infrastructure.Sorting.Interfaces
{
    public interface IListSortFactory<TQueryable, TRequest>
        where TQueryable : class
        where TRequest : class
    {
        SortDescriptor<TQueryable> DynamicSort(SortDescriptor<TQueryable> sortDescriptor, TRequest request);
    }
}
