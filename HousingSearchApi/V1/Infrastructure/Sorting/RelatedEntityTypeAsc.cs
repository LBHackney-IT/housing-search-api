using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using HousingSearchApi.V1.Interfaces.Sorting;
using Nest;
using System.Linq;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class RelatedEntityTypeAsc : ISort<QueryableProcess>
    {
        public SortDescriptor<QueryableProcess> GetSortDescriptor(SortDescriptor<QueryableProcess> descriptor)
        {
            return descriptor
                .Field(f => f.Field(p => SortingExtensions.OrderByTargetType(p, "person"))
                .Ascending())
                .Field(f => f.Field(p => SortingExtensions.OrderByTargetType(p, "tenure"))
                .Ascending());
        }
    }
}
