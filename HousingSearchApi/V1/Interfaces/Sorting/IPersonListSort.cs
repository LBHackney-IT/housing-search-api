using HousingSearchApi.V1.Domain.ElasticSearch;
using HousingSearchApi.V1.Factories;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public interface IPersonListSort
    {
        SortDescriptor<QueryablePerson> GetSortDescriptor(SortDescriptor<QueryablePerson> descriptor);
    }
}
