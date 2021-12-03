using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using HousingSearchApi.V1.Interfaces.Sorting;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Sorting
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
