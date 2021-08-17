using HousingSearchApi.V1.Gateways.Models;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public interface ISort<T> where T : class
    {
        SortDescriptor<T> GetSortDescriptor(SortDescriptor<T> descriptor);
    }
}
