using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public interface IQueryFactory
    {
        IQueryGenerator<T> CreateQuery<T>(HousingSearchRequest request) where T : class;
    }

    public class QueryFactory : IQueryFactory
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;

        public QueryFactory(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public IQueryGenerator<T> CreateQuery<T>(HousingSearchRequest request) where T : class
        {
            if (typeof(T) == typeof(QueryablePerson))
            {
                return (IQueryGenerator<T>)new PersonQueryGenerator(_wildCardAppenderAndPrepender);
            }

            if (typeof(T) == typeof(QueryableTenure))
            {
                return (IQueryGenerator<T>) new TenureQueryGenerator(_wildCardAppenderAndPrepender);
            }

            if (typeof(T) == typeof(QueryableAsset))
            {
                return (IQueryGenerator<T>) new AssetQueryGenerator(_wildCardAppenderAndPrepender);
            }


            throw new System.NotImplementedException($"Query type {typeof(T)} is not implemented");
        }
    }

    public class PersonQueryGenerator : IQueryGenerator<QueryablePerson>
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;

        public PersonQueryGenerator(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;

            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(request.SearchText);

            var searchSurnames = q.QueryString(m =>
                m.Query(string.Join(' ', listOfWildCardedWords))
                    .Fields(f => f.Field(p => p.Firstname).Field(p => p.Surname))
                    .Type(TextQueryType.MostFields));

            return searchSurnames;
        }
    }
}
