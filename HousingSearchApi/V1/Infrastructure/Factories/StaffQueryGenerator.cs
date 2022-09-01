using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Staffs;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Factories;
using Nest;
using System;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Infrastructure.Factories
{
    public class StaffQueryGenerator : IQueryGenerator<QueryableStaff>
    {
        private readonly IQueryBuilder<QueryableStaff> _queryBuilder;

        public StaffQueryGenerator(IQueryBuilder<QueryableStaff> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public QueryContainer Create<TRequest>(TRequest request, QueryContainerDescriptor<QueryableStaff> q)
        {

            if (!(request is GetStaffListRequest staffListRequest))
            {
                throw new ArgumentNullException($"{nameof(request).ToString()} shouldn't be null.");
            }

            _queryBuilder
                .WithWildstarQuery(staffListRequest.SearchText,
                    new List<string> { "emailAddress" })
                .WithExactQuery(staffListRequest.SearchText,
                    new List<string> { "emailAddress" }, new ExactSearchQuerystringProcessor());

            return _queryBuilder.Build(q);
        }
    }
}
