using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces;
using HousingSearchApi.V1.Interfaces.Factories;
using HousingSearchApi.V1.Interfaces.Filtering;
using HousingSearchApi.V1.Interfaces.Sorting;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Infrastructure
{
    public class ElasticSearchWrapper : IElasticSearchWrapper
    {
        private readonly IElasticClient _esClient;
        private readonly IQueryFactory _queryFactory;
        private readonly IPagingHelper _pagingHelper;
        private readonly ISortFactory _sortFactory;
        private readonly IFilterFactory _filterFactory;
        private readonly ILogger<ElasticSearchWrapper> _logger;
        private readonly IIndexSelector _indexSelector;

        public ElasticSearchWrapper(IElasticClient esClient, IQueryFactory queryFactory,
            IPagingHelper pagingHelper, ISortFactory sortFactory, ILogger<ElasticSearchWrapper> logger, IIndexSelector indexSelector,
            IFilterFactory filterFactory)
        {
            _esClient = esClient;
            _queryFactory = queryFactory;
            _pagingHelper = pagingHelper;
            _sortFactory = sortFactory;
            _filterFactory = filterFactory;
            _logger = logger;
            _indexSelector = indexSelector;
        }

        public async Task<ISearchResponse<T>> Search<T, TRequest>(TRequest request) where T : class where TRequest : class
        {
            try
            {
                var esNodes = string.Join(';', _esClient.ConnectionSettings.ConnectionPool.Nodes.Select(x => x.Uri));
                _logger.LogDebug($"ElasticSearch Search begins {esNodes}");

                if (request == null)
                    return new SearchResponse<T>();

                var searchRequest = (HousingSearchRequest) (object) request;

                var pageOffset = _pagingHelper.GetPageOffset(searchRequest.PageSize, searchRequest.Page);

                var result = await _esClient.SearchAsync<T>(x => x.Index(_indexSelector.Create<T>())
                    .Query(q => BaseQuery<T>().Create(request, q))
                    .PostFilter(q => _filterFactory.Create<T, TRequest>(request).GetDescriptor(q, request))
                    .Sort(_sortFactory.Create<T, TRequest>(request).GetSortDescriptor)
                    .Size(searchRequest.PageSize)
                    .Skip(pageOffset)
                    .TrackTotalHits()).ConfigureAwait(false);

                _logger.LogDebug("ElasticSearch Search ended");

                return result;
            }
            catch (ElasticsearchClientException e)
            {
                _logger.LogError(e, "ElasticSearch Search threw an ElasticSearchClientException. DebugInfo: " + e.DebugInformation);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ElasticSearch Search threw an exception");
                throw;
            }
        }

        public async Task<ISearchResponse<T>> SearchSets<T, TRequest>(TRequest request) where T : class where TRequest : class
        {
            var esNodes = string.Join(';', _esClient.ConnectionSettings.ConnectionPool.Nodes.Select(x => x.Uri));
            _logger.LogDebug($"ElasticSearch Search Sets begins {esNodes}");

            if (request == null)
                return new SearchResponse<T>();
            var searchRequest = (GetAllAssetListRequest) (object) request;

            var elements = !string.IsNullOrEmpty(searchRequest.LastHitId) ? new string[] { searchRequest.LastHitId } : new string[] { string.Empty };
            var lastSortedItem = !string.IsNullOrEmpty(searchRequest.LastHitId) ? elements.Cast<object>().ToArray() : null;

            ISearchResponse<T> result = null;

            try
            {
                if (string.IsNullOrEmpty(searchRequest.LastHitId) && searchRequest.Page == 1)
                {
                    result = await _esClient.SearchAsync<T>(x => x.Index(_indexSelector.Create<T>())
                      .Query(q => BaseQuery<T>().Create(request, q))
                      .Size(searchRequest.PageSize)
                      .Sort(_sortFactory.Create<T, TRequest>(request).GetSortDescriptor)
                      .TrackTotalHits()
                      ).ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(searchRequest.LastHitId))
                {
                    result = await _esClient.SearchAsync<T>(x => x.Index(_indexSelector.Create<T>())
                      .Query(q => BaseQuery<T>().Create(request, q))
                      .Size(searchRequest.PageSize)
                      .TrackTotalHits()
                      .SearchAfter(lastSortedItem)
                      .Sort(_sortFactory.Create<T, TRequest>(request).GetSortDescriptor)
                      ).ConfigureAwait(false);
                }

                _logger.LogDebug("ElasticSearch Search Sets ended");

                return result;
            }
            catch (Exception e)
            {

                _logger.LogError(e, "ElasticSearch Search Sets threw an exception");
                throw;
            }

        }
        public async Task<ISearchResponse<QueryableTenure>> SearchTenuresSets(GetAllTenureListRequest request)
        {
            var esNodes = string.Join(';', _esClient.ConnectionSettings.ConnectionPool.Nodes.Select(x => x.Uri));

            _logger.LogDebug($"ElasticSearch Search Tenures Sets begins {esNodes}");

            if (request == null)
            {
                return new SearchResponse<QueryableTenure>();
            }

            //last hit paging is only supported when sorting by tenure start date
            //any other type of paging is not supported. Page size can be set however when using start date sorting with last hit paging
            var sortDescriptor = _sortFactory.Create<QueryableTenure, GetAllTenureListRequest>(request).GetSortDescriptor;

            var lastSortedItem = GetLastSortedItem(request);

            ISearchResponse<QueryableTenure> result = null;

            try
            {
                result = await _esClient.SearchAsync<QueryableTenure>(
                    x => x.Index(_indexSelector.Create<QueryableTenure>())
                    .Query(q => BaseQuery<QueryableTenure>().Create(request, q))
                    .Size(request.PageSize)
                    .TrackTotalHits()
                    .SearchAfter(lastSortedItem)
                    .Sort(sortDescriptor)
                    ).ConfigureAwait(false);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ElasticSearch Search Tenures Sets threw an exception");
                throw;
            }
        }

        public static string[] GetLastSortedItem(GetAllTenureListRequest request)
        {
            var lastSortedItem =
                !string.IsNullOrEmpty(request.LastHitId) && !string.IsNullOrEmpty(request.LastHitTenureStartDate)
                    ? new string[] { request.LastHitTenureStartDate, request.LastHitId }
                    : null;

            return lastSortedItem;
        }

        private IQueryGenerator<T> BaseQuery<T>() where T : class
        {
            return _queryFactory.CreateQuery<T>();
        }


    }
}
