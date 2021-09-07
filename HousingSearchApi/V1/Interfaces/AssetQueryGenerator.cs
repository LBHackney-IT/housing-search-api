using System;
using System.Collections.Generic;
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

        public AssetQueryGenerator(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryableAsset> q)
        {
            if (!(request is HousingSearchRequest housingSearchRequest)) return null;

            var queryContainer = new QueryContainer();
            var filters = new List<Func<QueryContainerDescriptor<QueryableAsset>, QueryContainer>>();

            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(housingSearchRequest.SearchText);

            Func<QueryContainerDescriptor<QueryableAsset>, QueryContainer> filterBySearchTextContainer =
                (containerDescriptor) => containerDescriptor.QueryString(q => q.Query($"({string.Join(" AND ", listOfWildCardedWords)}) " + string.Join(' ', listOfWildCardedWords))
                    .Fields(f => f.Field("*"))
                    .Type(TextQueryType.MostFields));

            filters.Add(filterBySearchTextContainer);

            if (!string.IsNullOrWhiteSpace(housingSearchRequest.AssetTypes))
            {
                var types = housingSearchRequest.AssetTypes.Split(",");

                Func<QueryContainerDescriptor<QueryableAsset>, QueryContainer> filterByTypeContainer =
                    (containerDescriptor) => containerDescriptor.QueryString(q => q.Query(string.Join(' ', types))
                        .Fields(f => f.Field(asset => asset.AssetType))
                        .Type(TextQueryType.MostFields));

                filters.Add(filterByTypeContainer);
            }

            queryContainer = q.Bool(bq => bq.Filter(filters.ToArray()));

            return queryContainer;
        }
    }
}
