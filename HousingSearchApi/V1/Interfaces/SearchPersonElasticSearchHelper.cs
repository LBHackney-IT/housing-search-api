using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Interfaces.Sorting;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Interfaces
{
    public class SearchPersonElasticSearchHelper : ISearchPersonElasticSearchHelper
    {
        private readonly IElasticClient _esClient;
        private readonly ISearchPersonsQueryContainerOrchestrator _containerOrchestrator;
        private readonly IPagingHelper _pagingHelper;
        private readonly IPersonListSortFactory _iPersonListSortFactory;
        private readonly ILogger<SearchPersonElasticSearchHelper> _logger;
        private readonly Indices.ManyIndices _indices;

        public SearchPersonElasticSearchHelper(IElasticClient esClient, ISearchPersonsQueryContainerOrchestrator containerOrchestrator,
            IPagingHelper pagingHelper, IPersonListSortFactory iPersonListSortFactory,
            ILogger<SearchPersonElasticSearchHelper> logger)
        {
            _esClient = esClient;
            _containerOrchestrator = containerOrchestrator;
            _pagingHelper = pagingHelper;
            _iPersonListSortFactory = iPersonListSortFactory;
            _logger = logger;
            _indices = Indices.Index(new List<IndexName> { "persons" });
        }
        public async Task<ISearchResponse<QueryablePerson>> Search(GetPersonListRequest request)
        {
            try
            {
                var esNodes = string.Join(';', _esClient.ConnectionSettings.ConnectionPool.Nodes.Select(x => x.Uri));
                _logger.LogDebug($"ElasticSearch Search begins {esNodes}");
                if (request == null)
                    return new SearchResponse<QueryablePerson>();

                var pageOffset = _pagingHelper.GetPageOffset(request.PageSize, request.Page);

                var result = await _esClient.SearchAsync<QueryablePerson>(x => x.Index(_indices)
                    .Query(q => BaseQuery(request, q))
                    .Sort(_iPersonListSortFactory.Create(request).GetSortDescriptor)
                    .Size(request.PageSize)
                    .Skip(pageOffset)
                    .TrackTotalHits()).ConfigureAwait(false);

                _logger.LogDebug("ElasticSearch Search ended");

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ElasticSearch Search threw an exception");
                throw;
            }
        }

        private QueryContainer BaseQuery(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            return _containerOrchestrator.Create(request, q);
        }
    }
}
