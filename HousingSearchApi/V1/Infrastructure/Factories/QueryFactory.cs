using System;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using HousingSearchApi.V1.Interfaces.Factories;
using Microsoft.Extensions.DependencyInjection;
using QueryableTenure = Hackney.Shared.HousingSearch.Gateways.Models.Tenures.QueryableTenure;

namespace HousingSearchApi.V1.Infrastructure.Factories
{
    public class QueryFactory : IQueryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IQueryGenerator<T> CreateQuery<T>() where T : class
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
            if (typeof(T) == typeof(QueryableAccount))
            {
                return (IQueryGenerator<T>) new AccountQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableAccount>>());
            }

            throw new System.NotImplementedException($"Query type {typeof(T)} is not implemented");
        }
    }
}
