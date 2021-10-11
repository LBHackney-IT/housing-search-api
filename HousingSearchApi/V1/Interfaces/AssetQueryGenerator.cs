using System;
using System.Collections.Generic;
using System.Linq;
using Hackney.Core.ElasticSearch.Interfaces;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Assets;
using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public class AssetQueryGenerator : IQueryGenerator<QueryableAsset>
    {
        private readonly IQueryBuilder<QueryableAsset> _queryBuilder;

        public AssetQueryGenerator(IQueryBuilder<QueryableAsset> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryableAsset> q)
        {
            _queryBuilder.CreateWildstarSearchQuery(request.SearchText)
                .SpecifyFieldsToBeSearched(new List<string> { "assetAddress.addressLine1^2", "assetAddress.postCode", "assetAddress.uprn" });

            if (!string.IsNullOrWhiteSpace(request.AssetTypes))
            {
                _queryBuilder.CreateFilterQuery(request.AssetTypes)
                    .SpecifyFieldsToBeFiltered(new List<string> { "assetType"});
            }

            return _queryBuilder.FilterAndRespectSearchScore(q);
        }
    }
}
