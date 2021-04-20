using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public interface IPersonListSort
    {
        SortDescriptor<QueryablePerson> Get(SortDescriptor<QueryablePerson> descriptor);
    }
}
