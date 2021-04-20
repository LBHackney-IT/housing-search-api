using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Domain.ES;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public interface IPersonListSort
    {
        SortDescriptor<QueryablePerson> Get(SortDescriptor<QueryablePerson> descriptor);
    }
}
