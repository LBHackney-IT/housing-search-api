using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Interfaces
{
    public class SearchAssetsQueryContainerOrchestrator : ISearchQueryContainerOrchestrator<GetAssetListRequest, QueryableAsset>
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;

        public SearchAssetsQueryContainerOrchestrator(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public QueryContainer Create(GetAssetListRequest request, QueryContainerDescriptor<QueryableAsset> q)
        {
            QueryContainer result = new QueryContainer();

            if (string.IsNullOrWhiteSpace(request.SearchText))
            {
                result = q.Bool(bq => bq
                    .Must(mq => mq
                        .ConstantScore(cs => cs
                            .Filter(f => f.Term(field => field.AssetType, request.AssetType.ToString().ToLower())))));

                return result;
            }

            var listOfWildCardedWords = new List<string>();

            listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(request.SearchText);

            result = q.Bool(bq => bq
                .Filter(f => f.QueryString(m => m.Query(string.Join(' ', listOfWildCardedWords))
                    .Fields(f => f.Field("*"))
                    .Type(TextQueryType.MostFields)))
                .Must(mq => mq
                        .ConstantScore(cs => cs
                            .Filter(f => f.Term(field => field.AssetType, request.AssetType.ToString().ToLower())))));

            return result;
        }
    }
}
