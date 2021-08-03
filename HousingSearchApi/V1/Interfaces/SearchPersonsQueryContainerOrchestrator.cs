using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class SearchPersonsQueryContainerOrchestrator : ISearchPersonsQueryContainerOrchestrator
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;

        public SearchPersonsQueryContainerOrchestrator(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public QueryContainer Create(GetPersonListRequest request,
            QueryContainerDescriptor<QueryablePerson> q)
        {
            return new SearchPhrase(_wildCardAppenderAndPrepender).Create(request, q);
        }
    }
}
