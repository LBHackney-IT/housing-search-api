using System;
using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Factories;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Factories
{
    public class AccountQueryGenerator : IQueryGenerator<QueryableAccount>
    {
        private readonly IQueryBuilder<QueryableAccount> _queryBuilder;

        public AccountQueryGenerator(IQueryBuilder<QueryableAccount> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public QueryContainer Create<TRequest>(TRequest request, QueryContainerDescriptor<QueryableAccount> q)
        {
            switch (request)
            {
                case GetAccountListRequest accountListRequest:
                    {
                        if (!string.IsNullOrEmpty(accountListRequest.SearchText))
                        {
                            _queryBuilder
                                .WithWildstarQuery(accountListRequest.SearchText,
                                    new List<string> { "paymentReference", "tenure.fullAddress", "tenure.primaryTenants.fullName" })
                                .WithExactQuery(accountListRequest.SearchText,
                                    new List<string> { "paymentReference", "tenure.fullAddress", "tenure.primaryTenants.fullName" });
                        }

                        break;
                    }
                case GetAccountListByTenureIds tenureIds:
                    _queryBuilder.WithFilterQuery(string.Join(",", tenureIds.TenureIds), new List<string> { "targetId" });
                    break;
                default:
                    throw new ArgumentNullException(nameof(request));
            }

            return _queryBuilder.Build(q);
        }
    }
}
