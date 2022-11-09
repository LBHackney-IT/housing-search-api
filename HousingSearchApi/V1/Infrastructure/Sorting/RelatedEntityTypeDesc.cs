using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using HousingSearchApi.V1.Interfaces.Sorting;
using Nest;
using System.Linq;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class RelatedEntityTypeDesc : ISort<QueryableProcess>
    {
        public SortDescriptor<QueryableProcess> GetSortDescriptor(SortDescriptor<QueryableProcess> descriptor)
        {
            return descriptor
                .Field(f => f.Field(p => SortingExtensions.OrderByTargetType(p, "person"))
                .Descending())
                .Field(f => f.Field(p => SortingExtensions.OrderByTargetType(p, "tenure"))
                .Descending());
        }
    }
}
