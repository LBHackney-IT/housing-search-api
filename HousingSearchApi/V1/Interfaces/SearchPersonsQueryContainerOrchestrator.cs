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

        public QueryContainer CreatePerson(HousingSearchRequest request,
            QueryContainerDescriptor<QueryablePerson> q)
        {
            return new SearchPhrase(_wildCardAppenderAndPrepender).CreatePersonQuery(request, q);
        }

        public QueryContainer CreateTenure(HousingSearchRequest request, QueryContainerDescriptor<QueryableTenure> q)
        {
            return new SearchPhrase(_wildCardAppenderAndPrepender).CreateTenureQuery(request, q);
        }

        public QueryContainer CreateAsset(HousingSearchRequest request, QueryContainerDescriptor<QueryableAsset> queryContainerDescriptor)
        {
            throw new System.NotImplementedException();
        }
    }
}
