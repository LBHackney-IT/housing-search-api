using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using HousingSearchApi.V1.Interfaces.Sorting;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class PatchDesc : ISort<QueryableProcess>
    {
        public SortDescriptor<QueryableProcess> GetSortDescriptor(SortDescriptor<QueryableProcess> descriptor)
        {
            return descriptor
                .Descending(f => f.PatchAssignment.PatchName.Suffix("keyword"));
        }
    }
}
