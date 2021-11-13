using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.factories;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Factories
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

            return _queryBuilder
                .WithWildstarQuery(request.SearchText, new List<string>
                {
                    "paymentReference",
                    "tenuredAsset.fullAddress^3",
                    "householdMembers",
                    "householdMembers.fullName^3"
                }).Build(q);
        }
    }
}
