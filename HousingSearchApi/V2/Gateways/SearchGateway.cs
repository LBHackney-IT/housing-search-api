using System;
using System.Threading.Tasks;
using HousingSearchApi.V2.Domain.DTOs;
using HousingSearchApi.V2.Gateways.Interfaces;
using Nest;

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
                .Bool(b => b
                    .Should(
                        MultiMatchSingleField(searchParams.SearchText, boost: 6),
                        MultiMatchAcrossFields(searchParams.SearchText, boost: 2)
                    )
                )
            )
            .MinScore(15)
            .Size(searchParams.PageSize)
            .From((searchParams.PageNumber - 1) * searchParams.PageSize)
            .TrackTotalHits()
        );

        return new SearchResponseDto
        {
            Documents = searchResponse.Documents,
            Total = searchResponse.HitsMetadata.Total.Value,
        };
    }


    private Func<QueryContainerDescriptor<object>, QueryContainer> MultiMatchSingleField(string searchText, double boost) =>
        should => should
            .MultiMatch(mm => mm
                .Fields("*")
                .Query(searchText)
                .Type(TextQueryType.BestFields) // High score if all terms are in the same field
                .Operator(Operator.And) // All terms must be in the same field
                .Fuzziness(Fuzziness.Auto) // Allow for some typos
                .Boost(boost)
            );


    private Func<QueryContainerDescriptor<object>, QueryContainer> MultiMatchAcrossFields(string searchText, double boost) =>
        should => should
            .MultiMatch(mm => mm
                .Fields("*")
                .Query(searchText)
                .Type(TextQueryType.CrossFields) // High score if all terms are in any field
                .Operator(Operator.Or) // Terms can be across fields
                .Boost(boost)
            );

}
