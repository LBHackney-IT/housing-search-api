using Nest;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class DefaultSort : IPersonListSort
    {
        public SortDescriptor<QueryablePerson> Get(SortDescriptor<QueryablePerson> descriptor)
        {
            return descriptor;
        }
    }
}
