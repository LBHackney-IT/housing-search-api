using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using HousingSearchApi.V1.Interfaces.Sorting;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class TenureStartDateDesc : ISort<QueryableTenure>
    {
        public SortDescriptor<QueryableTenure> GetSortDescriptor(SortDescriptor<QueryableTenure> descriptor)
        {
            return descriptor.Descending(f => f.StartOfTenureDate);
        }
    }
}
