using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Tenures;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class TenureQueryGenerator : IQueryGenerator<QueryableTenure>
    {
        private readonly IQueryBuilder<QueryableTenure> _queryBuilder;

        public TenureQueryGenerator(IQueryBuilder<QueryableTenure> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryableTenure> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;

            return _queryBuilder.CreateWildstarSearchQuery(request.SearchText)
                .SpecifyFieldsToBeSearched(new List<string>
                {
                    "paymentReference",
                    "tenuredAsset.fullAddress^3",
                    "householdMembers",
                    "householdMembers.fullName^3"
                }).FilterAndRespectSearchScore(q);
        }
    }
}
