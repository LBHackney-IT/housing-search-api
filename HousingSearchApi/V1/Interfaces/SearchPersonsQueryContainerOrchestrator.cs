using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Factories;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class SearchPersonsQueryContainerOrchestrator : ISearchPersonsQueryContainerOrchestrator
    {
        public QueryContainer Create(GetPersonListRequest request,
            QueryContainerDescriptor<QueryablePerson> q)
        {
            return new SearchFirstNames().Create(request, q) ||
                   new SearchSurnames().Create(request, q);
        }
    }
}
