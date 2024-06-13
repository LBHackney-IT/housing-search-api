using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using HousingSearchApi.V1.Interfaces.Sorting;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class TenureStartDateDesc : ISort<QueryableTenure>
    {
        public SortDescriptor<QueryableTenure> GetSortDescriptor(SortDescriptor<QueryableTenure> descriptor)
        {
            // in the current schema the id field is defined as multi-field, so we need to use the id.keyword property to allow sorting
            // if the id field was defined as keyword then this wouldn't be necessary

            // both start date and id must be provided for this sorting to work
            return descriptor.Descending(x => x.StartOfTenureDate).Field("id.keyword", SortOrder.Descending);
        }
    }
}
