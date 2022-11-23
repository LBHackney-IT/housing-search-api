using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using Hackney.Shared.Processes.Domain;
using System;
using System.Linq;

namespace HousingSearchApi.V1.Infrastructure
{
    public static class SortingExtensions
    {
        public static Predicate<QueryableRelatedEntity> GetTenantDetails()
        {
            return x => x.TargetType == TargetType.person.ToString() && x.SubType == SubType.tenant.ToString();
        }
    }
}
