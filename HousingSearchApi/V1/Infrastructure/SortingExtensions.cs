using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using System.Linq;

namespace HousingSearchApi.V1.Infrastructure
{
    public static class SortingExtensions
    {
        public static string OrderByTargetType(QueryableProcess p, string targetType) =>
            p.RelatedEntities.First(x => x.TargetType == targetType)?.Description;
    }
}
