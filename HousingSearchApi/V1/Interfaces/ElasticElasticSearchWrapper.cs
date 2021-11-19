using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Sorting;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Interfaces
{
    public class ElasticElasticSearchWrapper : IElasticSearchWrapper
    {
        private readonly IElasticClient _esClient;
        private readonly IQueryFactory _queryFactory;
        private readonly IPagingHelper _pagingHelper;
        private readonly ISortFactory _iSortFactory;
        private readonly ILogger<ElasticElasticSearchWrapper> _logger;
        private readonly IIndexSelector _indexSelector;

        private static IHit<object> _lastHit;

        public ElasticElasticSearchWrapper(IElasticClient esClient, IQueryFactory queryFactory,
            IPagingHelper pagingHelper, ISortFactory iSortFactory, ILogger<ElasticElasticSearchWrapper> logger, IIndexSelector indexSelector)
        {
            _esClient = esClient;
            _queryFactory = queryFactory;
            _pagingHelper = pagingHelper;
            _iSortFactory = iSortFactory;
            _logger = logger;
            _indexSelector = indexSelector;
        }

        public async Task<ISearchResponse<T>> Search<T>(HousingSearchRequest request) where T : class
        {
            try
            {
                var esNodes = string.Join(';', _esClient.ConnectionSettings.ConnectionPool.Nodes.Select(x => x.Uri));
                _logger.LogDebug($"ElasticSearch Search begins {esNodes}");

                if (request == null)
                    return new SearchResponse<T>();

                var pageOffset = _pagingHelper.GetPageOffset(request.PageSize, request.Page);

                var result = await _esClient.SearchAsync<T>(x => x.Index(_indexSelector.Create<T>())
                    .Query(q => BaseQuery<T>(request).Create(request, q))
                    .Sort(_iSortFactory.Create<T>(request).GetSortDescriptor)
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

        public async Task<ISearchResponse<T>> SearchSets<T>(HousingSearchRequest request) where T : class
        {
            var esNodes = string.Join(';', _esClient.ConnectionSettings.ConnectionPool.Nodes.Select(x => x.Uri));
            _logger.LogDebug($"ElasticSearch Search Sets begins {esNodes}");

            if (request == null)
                return new SearchResponse<T>();
            
            var elements = _lastHit != null ? new string[] { _lastHit.Id } : new string[] { string.Empty };
            var lastSortedItem = _lastHit != null ? elements.Cast<object>().ToArray(): null;

            ISearchResponse<T> result = null ;

            try
            {
                if (_lastHit == null && request.Page == 1)
                {
                    result = await _esClient.SearchAsync<T>(x => x.Index(_indexSelector.Create<T>())
                      .Query(q => BaseQuery<T>(request).Create(request, q))
                      .Size(request.PageSize)
                      .Sort(_iSortFactory.Create<T>(request).GetSortDescriptor)
                      .TrackTotalHits()
                      ).ConfigureAwait(false);
                }
                else if(_lastHit != null)
                {
                    result = await _esClient.SearchAsync<T>(x => x.Index(_indexSelector.Create<T>())
                      .Query(q => BaseQuery<T>(request).Create(request, q))
                      .Size(request.PageSize)
                      .TrackTotalHits()
                      .SearchAfter(lastSortedItem)
                      .Sort(_iSortFactory.Create<T>(request).GetSortDescriptor)
                      ).ConfigureAwait(false);
                }

                _lastHit = result?.Documents.Count == request.PageSize ? result.Hits.Last() : null;

                _logger.LogDebug("ElasticSearch Search Sets ended");

                return result;
            }
            catch (Exception e)
            {

                _logger.LogError(e, "ElasticSearch Search Sets threw an exception");
                throw;
            }
            
        }

        private IQueryGenerator<T> BaseQuery<T>(HousingSearchRequest request) where T : class
        {
            return _queryFactory.CreateQuery<T>(request);
        }
    }
}
