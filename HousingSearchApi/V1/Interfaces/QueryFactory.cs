using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Assets;
using HousingSearchApi.V1.Gateways.Models.Persons;
using HousingSearchApi.V1.Gateways.Models.Tenures;
using HousingSearchApi.V1.Infrastructure;
using QueryableTenure = HousingSearchApi.V1.Gateways.Models.Tenures.QueryableTenure;

namespace HousingSearchApi.V1.Interfaces
{
    public class QueryFactory : IQueryFactory
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;
        private readonly IQueryBuilder<QueryablePerson> _personQueryBuilder;
        private readonly IQueryBuilder<QueryableAsset> _assetQueryBuilder;
        private readonly IQueryBuilder<QueryableTenure> _tenureQueryBuilder;

        public QueryFactory(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender,
            IQueryBuilder<QueryablePerson> personQueryBuilder,
            IQueryBuilder<QueryableAsset> assetQueryBuilder,
            IQueryBuilder<QueryableTenure> tenureQueryBuilder)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
            _personQueryBuilder = personQueryBuilder;
            _assetQueryBuilder = assetQueryBuilder;
            _tenureQueryBuilder = tenureQueryBuilder;
        }

        public IQueryGenerator<T> CreateQuery<T>(HousingSearchRequest request) where T : class
        {
            if (typeof(T) == typeof(QueryablePerson))
            {
                return (IQueryGenerator<T>) new PersonQueryGenerator(_personQueryBuilder, _wildCardAppenderAndPrepender);
            }

            if (typeof(T) == typeof(Gateways.Models.Tenures.QueryableTenure))
            {
                return (IQueryGenerator<T>) new TenureQueryGenerator(_tenureQueryBuilder, _wildCardAppenderAndPrepender);
            }

            if (typeof(T) == typeof(QueryableAsset))
            {
                return (IQueryGenerator<T>) new AssetQueryGenerator(_assetQueryBuilder, _wildCardAppenderAndPrepender);
            }


            throw new System.NotImplementedException($"Query type {typeof(T)} is not implemented");
        }
    }
}
