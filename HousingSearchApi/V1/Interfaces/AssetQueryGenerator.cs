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

        public AssetQueryGenerator(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryableAsset> q)
        {
            var filters = new List<Func<QueryContainerDescriptor<QueryableAsset>, QueryContainer>>();

            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(request.SearchText);

            var nonWildCardWords = request.SearchText.Split(" ").ToList();
            nonWildCardWords = nonWildCardWords.Select(x => "\"" + x + "\"").ToList();
            nonWildCardWords.Add("\"" + request.SearchText + "\"");

            #region Filter definitions
            QueryContainer FilterBySearchTextContainer(QueryContainerDescriptor<QueryableAsset> containerDescriptor) =>
                containerDescriptor
                    .QueryString(qs => qs.Query($"({string.Join(" AND ", listOfWildCardedWords)}) " + string.Join(' ', listOfWildCardedWords))
                        .Fields(f => f.Field("assetAddress.addressLine1^2")
                            .Field("assetAddress.postCode")
                            .Field("assetAddress.uprn"))
                    .Type(TextQueryType.MostFields));

            QueryContainer FilterByTypeContainer(QueryContainerDescriptor<QueryableAsset> containerDescriptor) =>
                containerDescriptor
                    .QueryString(qs => qs.Query(string.Join(' ', request.AssetTypes.Split(",")))
                    .Fields(f => f.Field(asset => asset.AssetType))
                    .Type(TextQueryType.MostFields));
            #endregion

            filters.Add(FilterBySearchTextContainer);
            if (!string.IsNullOrWhiteSpace(request.AssetTypes))
                filters.Add(FilterByTypeContainer);

            return q.Bool(bq => bq.Must(filters.ToArray()));
        }
    }
}
