using System;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Interfaces.Factories;
using Microsoft.Extensions.DependencyInjection;
using QueryableTenure = Hackney.Shared.HousingSearch.Gateways.Models.Tenures.QueryableTenure;
using QueryablePerson = Hackney.Shared.HousingSearch.Gateways.Models.Persons.QueryablePerson;
using HousingSearchApi.V1.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Staffs;
using Hackney.Shared.HousingSearch.Gateways.Models.Processes;

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
                return (IQueryGenerator<T>) new AssetQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableAsset>>(),
                    _serviceProvider.GetService<IFilterQueryBuilder<QueryableAsset>>());
            }
            if (typeof(T) == typeof(QueryableAccount))
            {
                return (IQueryGenerator<T>) new AccountQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableAccount>>());
            }

            if (typeof(T) == typeof(QueryableTransaction))
            {
                return (IQueryGenerator<T>) new TransactionsQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableTransaction>>());
            }
            if (typeof(T) == typeof(QueryableStaff))
            {
                return (IQueryGenerator<T>) new StaffQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableStaff>>());
            }
            if (typeof(T) == typeof(QueryableProcess))
            {
                return (IQueryGenerator<T>) new ProcessesQueryGenerator(_serviceProvider.GetService<IQueryBuilder<QueryableProcess>>());
            }
            throw new System.NotImplementedException($"Query type {typeof(T)} is not implemented");
        }
    }
}
