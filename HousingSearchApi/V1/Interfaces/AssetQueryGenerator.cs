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
            return _queryBuilder
                 .WithWildstarQuery(request.SearchText,
                     new List<string> { "assetAddress.addressLine1^2", "assetAddress.postCode", "assetAddress.uprn" })
                 .WithExactQuery(request.SearchText,
                     new List<string>
                     {
                        "assetAddress.addressLine1.keyword", "assetAddress.postCode.keyword", "assetAddress.keyword"
                     })
                 .WithFilterQuery(request.AssetTypes, new List<string> { "assetType.keyword" })
                 .Build(q);
        }
    }
}
