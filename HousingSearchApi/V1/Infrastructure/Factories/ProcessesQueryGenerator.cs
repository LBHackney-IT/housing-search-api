using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Factories;
using Nest;
using System;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Infrastructure.Factories
{
    public class ProcessesQueryGenerator : IQueryGenerator<QueryableProcess>
    {
        private readonly IQueryBuilder<QueryableProcess> _queryBuilder;

        public ProcessesQueryGenerator(IQueryBuilder<QueryableProcess> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public QueryContainer Create<TRequest>(TRequest request, QueryContainerDescriptor<QueryableProcess> q)
        {

            if (!(request is GetProcessListRequest processListRequest))
            {
                throw new ArgumentNullException($"{nameof(request).ToString()} shouldn't be null.");
            }

            _queryBuilder
                .WithWildstarQuery(processListRequest.SearchText,
                    new List<string> { "targetId" })
                .WithExactQuery(processListRequest.SearchText,
                    new List<string> { "targetId" }, new ExactSearchQuerystringProcessor())
                .WithFilterQuery(processListRequest.TargetType.ToString(), new List<string> { "targetType" })
                .WithFilterQuery(processListRequest.TargetId.ToString(), new List<string> { "targetId" });

            return _queryBuilder.Build(q);
        }
    }
}
