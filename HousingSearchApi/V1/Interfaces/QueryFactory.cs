using System;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.QueryGenerators;
using Microsoft.Extensions.DependencyInjection;
using QueryablePerson = Hackney.Shared.HousingSearch.Gateways.Models.Persons.QueryablePerson;

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

            if (typeof(T) == typeof(QueryableTenure))
            {
                return (IQueryGenerator<T>) new TenureQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableTenure>>());
            }

            if (typeof(T) == typeof(QueryableAsset))
            {
                return (IQueryGenerator<T>) new AssetQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableAsset>>());
            }

            if (typeof(T) == typeof(QueryableTransaction))
            {
                return (IQueryGenerator<T>) new TransactionsQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableTransaction>>());
            }

            throw new System.NotImplementedException($"Query type {typeof(T)} is not implemented");
        }
    }
}
