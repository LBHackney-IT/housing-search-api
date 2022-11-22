using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using Hackney.Shared.Processes.Domain;
using HousingSearchApi.V1.Interfaces.Sorting;
using Nest;
using System;
using System.Linq;

namespace HousingSearchApi.V1.Infrastructure.Sorting
{
    public class PersonNameDesc : ISort<QueryableProcess>
    {
        public SortDescriptor<QueryableProcess> GetSortDescriptor(SortDescriptor<QueryableProcess> descriptor)
        {
            return descriptor
                .Descending(f => f.RelatedEntities.Find(SortingExtensions.GetPersonDetails()).Description);
        }
    }
}
