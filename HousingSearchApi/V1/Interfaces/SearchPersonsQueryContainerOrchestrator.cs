using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class SearchPersonsQueryContainerOrchestrator : ISearchPersonsQueryContainerOrchestrator
    {
        private readonly IQueryFactory _queryFactory;

        public SearchPersonsQueryContainerOrchestrator(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public QueryContainer CreatePerson(HousingSearchRequest request,
            QueryContainerDescriptor<QueryablePerson> query)
        {
            return _queryFactory.CreateQuery<QueryablePerson>(request).Create(request, query);
        }

        public QueryContainer CreateTenure(HousingSearchRequest request, QueryContainerDescriptor<QueryableTenure> query)
        {
            return _queryFactory.CreateQuery<QueryableTenure>(request).Create(request, query);
        }

        public QueryContainer CreateAsset(HousingSearchRequest request, QueryContainerDescriptor<QueryableAsset> query)
        {
            return _queryFactory.CreateQuery<QueryableAsset>(request).Create(request, query);
        }
    }
}
