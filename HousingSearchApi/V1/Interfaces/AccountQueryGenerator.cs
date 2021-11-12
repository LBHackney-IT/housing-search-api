using System;
using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using HousingSearchApi.V1.Boundary.Requests;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class AccountQueryGenerator : IQueryGenerator<QueryableAccount>
    {
        private readonly IQueryBuilder<QueryableAccount> _queryBuilder;

        public AccountQueryGenerator(IQueryBuilder<QueryableAccount> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryableAccount> q)
        {
            if (request == null)
                throw new ArgumentNullException($"{nameof(request).ToString()} shouldn't be null.");

            if (!string.IsNullOrEmpty(request.SearchText))
                _queryBuilder
                    .WithWildstarQuery(request.SearchText,
                        new List<string> { "paymentReference", "tenure.fullAddress", "tenure.primaryTenants.fullName" })
                    .WithExactQuery(request.SearchText,
                        new List<string> { "paymentReference", "tenure.fullAddress", "tenure.primaryTenants.fullName" });

            return _queryBuilder.Build(q);
        }
    }
}
