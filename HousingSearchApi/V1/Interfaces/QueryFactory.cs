using System;
using Hackney.Core.ElasticSearch.Interfaces;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Assets;
using HousingSearchApi.V1.Gateways.Models.Persons;
using Microsoft.Extensions.DependencyInjection;

namespace HousingSearchApi.V1.Interfaces
{
    public class QueryFactory : IQueryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IQueryGenerator<T> CreateQuery<T>(HousingSearchRequest request) where T : class
        {
            if (typeof(T) == typeof(QueryablePerson))
            {
                return (IQueryGenerator<T>) new PersonQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryablePerson>>());
            }

            if (typeof(T) == typeof(Gateways.Models.Tenures.QueryableTenure))
            {
                return (IQueryGenerator<T>) new TenureQueryGenerator(_serviceProvider.GetService<IQueryBuilder<Gateways.Models.Tenures.QueryableTenure>>());
            }

            if (typeof(T) == typeof(QueryableAsset))
            {
                return (IQueryGenerator<T>) new AssetQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableAsset>>());
            }


            throw new System.NotImplementedException($"Query type {typeof(T)} is not implemented");
        }
    }
}
