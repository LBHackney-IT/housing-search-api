using System;
using System.Collections.Generic;
using System.Linq;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Models.Assets;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class AssetQueryGenerator : IQueryGenerator<QueryableAsset>
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;
        private readonly IQueryBuilder<QueryableAsset> _queryBuilder;

        public AssetQueryGenerator(IQueryBuilder<QueryableAsset> queryBuilder, IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
            _queryBuilder = queryBuilder;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryableAsset> containerDescriptor)
        {

            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(request.SearchText);
            var searchQuery = $"({string.Join(" AND ", listOfWildCardedWords)}) " +
                              string.Join(' ', listOfWildCardedWords);
            var searchFields = new List<string> { "assetAddress.addressLine1^2", "assetAddress.postCode", "assetAddress.uprn" };
            _queryBuilder.WithQueryAndFields(searchQuery, searchFields);

            if (!string.IsNullOrWhiteSpace(request.AssetTypes))
            {
                var filterQuery = string.Join(' ', request.AssetTypes.Split(","));
                var filterFields = new List<string> { "assetType" };
                _queryBuilder.WithQueryAndFields(filterQuery, filterFields);
            }

            return _queryBuilder.FilterAndRespectSearchScore(containerDescriptor);
        }
    }
}
