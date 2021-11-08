using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HousingSearchApi.V1.Infrastructure.Helpers.Interfaces;
using HousingSearchApi.V1.Infrastructure.Interfaces;
using HousingSearchApi.V1.Infrastructure.Sorting.Interfaces;
using Microsoft.Extensions.Logging;
using Nest;

namespace AccountsApi.V1.Infrastructure.Helpers
{
    public class SearchElasticSearchHelper<TQueryable, TRequest> : ISearchElasticSearchHelper<TQueryable, TRequest>
        where TQueryable : class
        where TRequest : class
    {
        private readonly IElasticClient _esClient;
        private readonly ISearchQueryContainerOrchestrator<TRequest,TQueryable> _containerOrchestrator;
        private readonly IPagingHelper _pagingHelper;
        private readonly IListSortFactory<TRequest, TQueryable> _listSortFactory;
        private readonly ILogger<SearchElasticSearchHelper<TRequest, TQueryable>> _logger;
        private readonly Indices.ManyIndices _indices;

        public SearchElasticSearchHelper(IElasticClient esClient, ISearchQueryContainerOrchestrator<TRequest,TQueryable> containerOrchestrator,
            IPagingHelper pagingHelper, IListSortFactory<TRequest, TQueryable> listSortFactory, ILogger<SearchElasticSearchHelper<TRequest, TQueryable>> logger)
        {
            _esClient = esClient;
            _containerOrchestrator = containerOrchestrator;
            _pagingHelper = pagingHelper;
            _listSortFactory = listSortFactory;
            _logger = logger;
            _indices = Indices.Index(new List<IndexName> { "accounts" });
        }

        public async Task<ISearchResponse<TQueryable>> Search(TRequest request)
        {
            try
            {
                _logger.LogDebug($"ElasticSearch Search begins {Environment.GetEnvironmentVariable("ELASTICSEARCH_DOMAIN_URL")}");

                if (request == null)
                {
                    return new SearchResponse<TQueryable>();
                }

                var pageOffset = _pagingHelper.GetPageOffset(request.PageSize, request.PageNumber);

                var result = await _esClient.SearchAsync<TQueryable>(x => x.Index(_indices)
                    .Query(q => BaseQuery(request, q))
                    .Sort(s => _listSortFactory.DynamicSort(s, request))
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

        private QueryContainer BaseQuery(TRequest request, QueryContainerDescriptor<TQueryable> queryDescriptor)
        {
            return _containerOrchestrator.Create(request, queryDescriptor);
        }

    }
}
