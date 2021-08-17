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
        private readonly IQueryFactory _queryFactory;
        private readonly IPagingHelper _pagingHelper;
        private readonly IPersonListSortFactory _iPersonListSortFactory;
        private readonly ILogger<SearchPersonElasticSearchHelper> _logger;
        private readonly Indices.ManyIndices _personIndices;
        private readonly Indices.ManyIndices _tenureIndices;

        public SearchPersonElasticSearchHelper(IElasticClient esClient, IQueryFactory queryFactory,
            IPagingHelper pagingHelper, IPersonListSortFactory iPersonListSortFactory, ILogger<SearchPersonElasticSearchHelper> logger)
        {
            _esClient = esClient;
            _queryFactory = queryFactory;
            _pagingHelper = pagingHelper;
            _iPersonListSortFactory = iPersonListSortFactory;
            _logger = logger;
            _personIndices = Indices.Index(new List<IndexName> { "persons" });
            _tenureIndices = Indices.Index(new List<IndexName> { "tenures" });
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

                var result = await _esClient.SearchAsync<T>(x => x.Index(_tenureIndices)
                    .Query(q => BaseQuery<T>(request).Create(request, q))
                    .Sort(_iPersonListSortFactory.Create<T>(request).GetSortDescriptor)
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

        private IQueryGenerator<T> BaseQuery<T>(HousingSearchRequest request) where T : class
        {
            return _queryFactory.CreateQuery<T>(request);
        }
    }
}
