using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using Hackney.Shared.Processes.Domain.Constants;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces;
using HousingSearchApi.V1.Interfaces.Factories;
using Nest;
using System;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Infrastructure.Factories
{
    public class ProcessesQueryGenerator : IQueryGenerator<QueryableProcess>
    {
        private readonly IFilterQueryBuilder<QueryableProcess> _queryBuilder;

        public ProcessesQueryGenerator(IFilterQueryBuilder<QueryableProcess> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        private static string ConstructIsOpenFilter(GetProcessListRequest processListRequest)
        {
            var closedStates = $"{SharedStates.ProcessClosed} | {SharedStates.ProcessCompleted} | {SharedStates.ProcessCancelled}";
            return processListRequest.IsOpen.Value ? $"NOT ({closedStates})" : closedStates;
        }

        public QueryContainer Create<TRequest>(TRequest request, QueryContainerDescriptor<QueryableProcess> q)
        {

            if (!(request is GetProcessListRequest processListRequest))
            {
                throw new ArgumentNullException($"{nameof(request).ToString()} shouldn't be null.");
            }

            //if (processListRequest.SearchText == null)
            //{
            //    _queryBuilder
            //        .WithExactQuery(processListRequest.TargetId?.ToString(),
            //            new List<string> { "targetId" })
            //        .WithFilterQuery(processListRequest.TargetType, new List<string> { "targetType" })
            //        .WithFilterQuery(processListRequest.ProcessNames, new List<string> { "processName" });
            //}
            //else
            //{

            //    _queryBuilder
            //        .WithExactQuery(processListRequest.SearchText,
            //            new List<string> { "patchAssignment.patchId" })
            //        .WithFilterQuery(processListRequest.TargetId?.ToString(),
            //            new List<string> { "targetId" })
            //        .WithFilterQuery(processListRequest.TargetType, new List<string> { "targetType" })
            //        .WithFilterQuery(processListRequest.ProcessNames, new List<string> { "processName" });
            //}

            if (processListRequest.TargetId.HasValue)
            {
                _queryBuilder
                    .WithExactQuery(processListRequest.TargetId?.ToString(),
                        new List<string> { "targetId" }, new ExactSearchQuerystringProcessor())
                    .WithFilterQuery(processListRequest.TargetType, new List<string> { "targetType" })
                    .WithFilterQuery(processListRequest.ProcessNames, new List<string> { "processName" });
            }
            else
            {

                _queryBuilder
                    .WithExactQuery(processListRequest.SearchText,
                        new List<string> { "patchAssignment.patchId" })
                    .WithFilterQuery(processListRequest.ProcessNames, new List<string> { "processName" });
            }

            if (processListRequest.IsOpen.HasValue)
                _queryBuilder.WithFilterQuery(ConstructIsOpenFilter(processListRequest), new List<string> { "state" });

            return _queryBuilder.Build(q);
        }


    }
}



