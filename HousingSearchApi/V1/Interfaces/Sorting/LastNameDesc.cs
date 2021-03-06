using HousingSearchApi.V1.Gateways.Models.Persons;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class SurnameDesc : ISort<QueryablePerson>
    {
        public SortDescriptor<QueryablePerson> GetSortDescriptor(SortDescriptor<QueryablePerson> descriptor)
        {
            return descriptor
                .Descending(f => f.Surname)
                .Descending(f => f.Firstname);
        }
    }
}
