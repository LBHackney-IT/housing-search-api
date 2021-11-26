using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using HousingSearchApi.V1.Interfaces.Sorting;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class SurnameAsc : ISort<QueryablePerson>
    {
        public SortDescriptor<QueryablePerson> GetSortDescriptor(SortDescriptor<QueryablePerson> descriptor)
        {
            return descriptor
                .Ascending(f => f.Surname)
                .Ascending(f => f.Firstname);
        }
    }
}
