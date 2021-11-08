using System;
using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Domain.QueryableModels;
using HousingSearchApi.V1.Infrastructure.Interfaces;
using Nest;

namespace HousingSearchApi.V1.Infrastructure
{
    public class SearchAccountQueryContainerOrchestrator : ISearchQueryContainerOrchestrator<GetAccountListRequest, QueryableAccount>
    {
        private readonly IQueryBuilder<QueryableAccount> _builder;

        public SearchAccountQueryContainerOrchestrator(IQueryBuilder<QueryableAccount> builder)
        {
            _builder = builder;
        } 

        public QueryContainer Create(GetAccountListRequest request, QueryContainerDescriptor<QueryableAccount> q)
        {
            if (request == null)
                throw new ArgumentNullException($"{nameof(request).ToString()} shouldn't be null.");

            if (!string.IsNullOrEmpty(request.SearchText))
                _builder
                    .WithWildstarQuery(request.SearchText,
                        new List<string> { "paymentReference", "tenure.fullAddress", "tenure.primaryTenants.fullName" })
                    .WithExactQuery(request.SearchText,
                        new List<string> { "paymentReference", "tenure.fullAddress", "tenure.primaryTenants.fullName" });

            return _builder.Build(q);
        }
    }
}
