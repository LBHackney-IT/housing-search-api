using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Sorting;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;
using HousingSearchApi.V1.Interfaces;
using HousingSearchApi.V1.Interfaces.Factories;

namespace HousingSearchApi.V1.Infrastructure
{
    public class ElasticSearchWrapper : IElasticSearchWrapper
    {
        private readonly IElasticClient _esClient;
        private readonly IQueryFactory _queryFactory;
        private readonly IPagingHelper _pagingHelper;
        private readonly ISortFactory _iSortFactory;
        private readonly ILogger<ElasticSearchWrapper> _logger;
        private readonly IIndexSelector _indexSelector;

        public ElasticSearchWrapper(IElasticClient esClient, IQueryFactory queryFactory,
            IPagingHelper pagingHelper, ISortFactory iSortFactory, ILogger<ElasticSearchWrapper> logger, IIndexSelector indexSelector)
        {
            _esClient = esClient;
            _queryFactory = queryFactory;
            _pagingHelper = pagingHelper;
            _iSortFactory = iSortFactory;
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

                HousingSearchRequest searchRequest = (HousingSearchRequest) (object) request;

                var pageOffset = _pagingHelper.GetPageOffset(searchRequest.PageSize, searchRequest.Page);

                var result = await _esClient.SearchAsync<T>(x => x.Index(_indexSelector.Create<T>())
                    .Query(q => BaseQuery<T>().Create(request, q))
                    .Sort(_iSortFactory.Create<T, TRequest>(request).GetSortDescriptor)
                    .Size(searchRequest.PageSize)
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

        private IQueryGenerator<T> BaseQuery<T>() where T : class
        {
            return _queryFactory.CreateQuery<T>();
        }
    }
}
