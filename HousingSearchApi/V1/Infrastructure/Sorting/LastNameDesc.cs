using Nest;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class SurnameDesc : IPersonListSort
    {
        public SortDescriptor<QueryablePerson> Get(SortDescriptor<QueryablePerson> descriptor)
        {
            return descriptor
                .Descending(f => f.Surname)
                .Descending(f => f.Firstname);
        }
    }
}
