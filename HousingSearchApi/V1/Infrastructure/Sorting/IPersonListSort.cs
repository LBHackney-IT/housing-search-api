using Nest;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public interface IPersonListSort
    {
        SortDescriptor<QueryablePerson> Get(SortDescriptor<QueryablePerson> descriptor);
    }
}
