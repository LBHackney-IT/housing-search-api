using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Interfaces.Sorting;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class SearchPersonElasticSearchHelper : ISearchPersonElasticSearchHelper
    {
        private IElasticClient _esClient;
        private readonly ISearchPersonsQueryContainerOrchestrator _containerOrchestrator;
        private readonly IPagingHelper _pagingHelper;
        private readonly IPersonListSortFactory _iPersonListSortFactory;
        private Indices.ManyIndices _indices;

        public SearchPersonElasticSearchHelper(IElasticClient esClient, ISearchPersonsQueryContainerOrchestrator containerOrchestrator,
            IPagingHelper pagingHelper, IPersonListSortFactory iPersonListSortFactory)
        {
            _esClient = esClient;
            _containerOrchestrator = containerOrchestrator;
            _pagingHelper = pagingHelper;
            _iPersonListSortFactory = iPersonListSortFactory;
            _indices = Indices.Index(new List<IndexName> { "persons" });
        }
        public async Task<ISearchResponse<QueryablePerson>> Search(GetPersonListRequest request)
        {
            try
            {
                LambdaLogger.Log("ElasticSearch Search begins " + Environment.GetEnvironmentVariable("ELASTICSEARCH_DOMAIN_URL"));
                if (request == null)
                    return new SearchResponse<QueryablePerson>();

                var pageOffset = _pagingHelper.GetPageOffset(request.PageSize, request.Page);

                var result = await _esClient.SearchAsync<QueryablePerson>(x => x.Index(_indices)
                    .Query(q => BaseQuery(request, q))
                    .Sort(_iPersonListSortFactory.Create(request).GetSortDescriptor)
                    .Size(request.PageSize)
                    .Skip(pageOffset)
                    .TrackTotalHits()).ConfigureAwait(false);

                LambdaLogger.Log("ElasticSearch Search ended");

                return result;
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);

                throw new Exception(e.Message);
            }
        }

        private QueryContainer BaseQuery(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            return _containerOrchestrator.Create(request, q);
        }
    }
}
