using System.Collections.Generic;
using HousingSearchApi.V2.Gateways.Interfaces;
using Nest;
using System.Threading.Tasks;
using HousingSearchApi.V2.Domain.DTOs;

namespace HousingSearchApi.V2.Gateways;

public class SearchGateway : ISearchGateway
{
    private readonly IElasticClient _elasticClient;

    public SearchGateway(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<IReadOnlyCollection<object>> Search(string index, SearchParametersDto searchParametersDto)
    {
        // Create query
        QueryContainer queryContainer;
        if (string.IsNullOrEmpty(searchParametersDto.SearchText))
            queryContainer = new MatchAllQuery();
        else
            queryContainer = new SimpleQueryStringQuery()
            {
                Query = searchParametersDto.SearchText,
            };
        // Create search request
        var searchRequest = new SearchRequest(index)
        {
            Query = queryContainer,
            Size = searchParametersDto.PageSize,
            Sort = new List<ISort> {
                new FieldSort
                {
                    Field = searchParametersDto.SortField,
                    Order = searchParametersDto.IsDesc ? SortOrder.Descending : SortOrder.Ascending
                }
            },
            From = searchParametersDto.PageNumber
        };

        var searchResponse = await _elasticClient.SearchAsync<object>(searchRequest).ConfigureAwait(false);
        return searchResponse.Documents;
    }
}


