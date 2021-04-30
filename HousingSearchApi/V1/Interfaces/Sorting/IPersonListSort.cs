using HousingSearchApi.V1.Gateways;
using HousingSearchApi.V1.Gateways.Models;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public interface IPersonListSort
    {
        SortDescriptor<QueryablePerson> GetSortDescriptor(SortDescriptor<QueryablePerson> descriptor);
    }
}
