using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Gateways
{
    public class SearchPersonsQueryContainerOrchestrator : ISearchPersonsQueryContainerOrchestrator
    {
        public QueryContainer Create(GetPersonListRequest request,
            QueryContainerDescriptor<QueryablePerson> q)
        {
            return new SearchFirstNames().Create(request, q) ||
                   new SearchLastNames().Create(request, q);
        }
    }
}
