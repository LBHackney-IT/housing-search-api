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
        string fieldName;
        if (indexName == "assets")
            fieldName = "assetAddress.addressLine1";
        else if (indexName == "tenures")
            fieldName = "tenuredAsset.fullAddress";
        else if (indexName == "persons")
            fieldName = "tenures.assetFullAddress";
        else
            throw new Exception($"Index name '{indexName}' is not supported");

        var searchResponse = await _elasticClient.SearchAsync<object>(s => s
            .Index(indexName)
            .Query(q => q
                .Bool(b => b
                    .Should(
                        MatchPhrasePrefix(searchParams.SearchText, boost: 10, fieldName: fieldName),
                        MultiMatchSingleField(searchParams.SearchText, boost: 6),
                        MultiMatchCrossFields(searchParams.SearchText, boost: 2),
                        MultiMatchMostFields(searchParams.SearchText, boost: 1)
                    )
                )
            )
            .MinScore(25)
            .Size(searchParams.PageSize)
            .From((searchParams.Page - 1) * searchParams.PageSize)
            .TrackTotalHits()
        );

        if (!searchResponse.IsValid)
            throw new Exception($"Elasticsearch search failed: {searchResponse.DebugInformation}");

        return new SearchResponseDto
        {
            Documents = searchResponse.Documents,
            Total = searchResponse.HitsMetadata.Total.Value,
        };
    }


    private Func<QueryContainerDescriptor<object>, QueryContainer>
        MatchPhrasePrefix(string searchText, double boost, string fieldName) =>
        should => should
            .MatchPhrasePrefix(mp => mp
                .Field(fieldName)
                .Query(searchText)
                .Boost(boost)
            );

    // Score for matching a single (best) field
    private Func<QueryContainerDescriptor<object>, QueryContainer>
        MultiMatchSingleField(string searchText, double boost) =>
        should => should
            .MultiMatch(mm => mm
                .Fields("*")
                .Query(searchText)
                .Type(TextQueryType.BestFields)
                .Operator(Operator.And)
                .Fuzziness(Fuzziness.Auto)
                .Boost(boost)
            );

    // Score for matching the combination of many fields
    private Func<QueryContainerDescriptor<object>, QueryContainer>
        MultiMatchCrossFields(string searchText, double boost) =>
        should => should
            .MultiMatch(mm => mm
                .Fields("*")
                .Query(searchText)
                .Type(TextQueryType.CrossFields)
                .Operator(Operator.Or)
                .Boost(boost)
            );

    // Score for matching a high number (quantity) of fields
    private Func<QueryContainerDescriptor<object>, QueryContainer> MultiMatchMostFields(string searchText, double boost) =>
        should => should
            .MultiMatch(mm => mm
                .Fields("*")
                .Query(searchText)
                .Type(TextQueryType.MostFields)
                .Operator(Operator.Or)
                .Fuzziness(Fuzziness.Auto)
                .Boost(boost)
            );
}
