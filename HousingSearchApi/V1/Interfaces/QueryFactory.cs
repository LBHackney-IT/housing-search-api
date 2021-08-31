using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Assets;
using HousingSearchApi.V1.Gateways.Models.Persons;
using HousingSearchApi.V1.Gateways.Models.Tenures;
using HousingSearchApi.V1.Infrastructure;

namespace HousingSearchApi.V1.Interfaces
{
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
                return (IQueryGenerator<T>) new PersonQueryGenerator(_wildCardAppenderAndPrepender);
            }

            if (typeof(T) == typeof(Gateways.Models.Tenures.QueryableTenure))
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
}
