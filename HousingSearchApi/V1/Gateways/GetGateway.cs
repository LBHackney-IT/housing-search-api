using System.Collections.Generic;
using HousingSearchApi.V1.Gateways.Interfaces;
using Nest;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Gateways
{
    public class GetGateway : IGetGateway
    {
        private readonly IElasticClient _elasticClient;

        public GetGateway(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<IReadOnlyCollection<object>> FreeSearch(string index, string searchText)
        {
            QueryContainer queryContainer;

            if (string.IsNullOrEmpty(searchText))
                queryContainer = new MatchAllQuery();
            else
                queryContainer = new QueryStringQuery
                {
                    Query = searchText
                };
            var searchRequest = new SearchRequest(index)
            {
                Query = queryContainer,
                Size = 40,
                Sort = new List<ISort> { new FieldSort { Field = "_score", Order = SortOrder.Descending } }
            };
            var searchResponse = await _elasticClient.SearchAsync<object>(searchRequest).ConfigureAwait(false);
            return searchResponse.Documents;
        }
    }
}

