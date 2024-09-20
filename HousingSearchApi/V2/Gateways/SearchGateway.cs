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

    public async Task<SearchResponseDto> Search(string indexName, SearchParametersDto searchParams)
    {
        var searchResponse = await _elasticClient.SearchAsync<object>(s => s
                .Index(indexName)
                .Query(q => q
                    .SimpleQueryString(qs => qs
                        .Fields("*")
                        .Query(searchParams.SearchText)
                    )
                )
                .Size(searchParams.PageSize)
                .From((searchParams.PageNumber - 1) * searchParams.PageSize)
                .TrackTotalHits(true)
        );

        return new SearchResponseDto
        {
            Documents = searchResponse.Documents,
            Total = searchResponse.HitsMetadata.Total.Value
        };
    }

}



// .Sort(so => so
//     .Field(f => f
//         .Field(searchParametersDto.SortField)
//         .Order(searchParametersDto.IsDesc ? SortOrder.Descending : SortOrder.Ascending)
//     )
// )
