using HousingSearchApi.V1.Interfaces.Sorting;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class DefaultSort<T> : ISort<T> where T : class
    {
        public SortDescriptor<T> GetSortDescriptor(SortDescriptor<T> descriptor)
        {
            return descriptor;
        }
    }
}
