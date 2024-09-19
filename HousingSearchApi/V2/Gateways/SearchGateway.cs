using System.Collections.Generic;
using System.Linq;
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

    public async Task<IReadOnlyCollection<object>> Search(string indexName, SearchParametersDto searchParametersDto)
    {
        var searchResponse = await _elasticClient.SearchAsync<object>(s => s
            .Index(indexName)
            .Query(q => q
                .SimpleQueryString(qs => qs
                    .Fields("*")
                    .Query(searchParametersDto.SearchText)
                )
            )
        );

        return searchResponse.Documents;
    }
}



// .Sort(so => so
//     .Field(f => f
//         .Field(searchParametersDto.SortField)
//         .Order(searchParametersDto.IsDesc ? SortOrder.Descending : SortOrder.Ascending)
//     )
// )
