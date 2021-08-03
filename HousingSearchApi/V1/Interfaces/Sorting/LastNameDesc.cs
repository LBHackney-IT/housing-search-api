using HousingSearchApi.V1.Gateways.Models;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class SurnameDesc : IPersonListSort
    {
        public SortDescriptor<QueryablePerson> GetSortDescriptor(SortDescriptor<QueryablePerson> descriptor)
        {
            return descriptor
                .Descending(f => f.Surname)
                .Descending(f => f.Firstname);
        }
    }
}
