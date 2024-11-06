using System;
using System.Collections.Generic;
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
        var shouldOperations = new List<Func<QueryContainerDescriptor<object>, QueryContainer>>() {
            SearchOperations.MultiMatchBestFields(searchParams.SearchText, boost: 6),
        };

        // Extend search operations depending on the index
        if (indexName == "assets")
        {
            Fields keywordFields = new[] {
                "id", "assetAddress.uprn", "propertyReference",
                "tenure.id", "tenure.paymentReference"
            };
            Field keyAddressField = "assetAddress.addressLine1";
            Fields addressFieldNames = new[] { "assetAddress.addressLine1", "assetAddress.addressLine2", "assetAddress.postCode" };
            shouldOperations.AddRange(new[]
            {
                SearchOperations.MultiMatchBestFields(searchParams.SearchText, fields: keywordFields, boost: 15),
                SearchOperations.MultiMatchCrossFields(searchParams.SearchText, fields: addressFieldNames, boost: 8),
                SearchOperations.MatchField(searchParams.SearchText, field: keyAddressField, boost: 10),
                SearchOperations.MatchPhrasePrefix(searchParams.SearchText, field: keyAddressField, boost: 8),
                SearchOperations.WildcardMatch(searchParams.SearchText, fields: new[] {keyAddressField}, boost: 15),
            });
        }
        else if (indexName == "tenures")
        {
            Field nameField = "householdMembers.fullName";
            Fields keywordFields = new[] {
                "id", "paymentReference",
                "tenuredAsset.id", "tenuredAsset.uprn"
            };
            Field addressField = "tenuredAsset.fullAddress";

            shouldOperations.AddRange(new[]
            {
                SearchOperations.MatchField(searchParams.SearchText, field: nameField, boost: 12),
                SearchOperations.WildcardMatch(searchParams.SearchText, fields: new [] {nameField}, boost: 10),
                SearchOperations.MultiMatchBestFields(searchParams.SearchText, fields: keywordFields, boost: 12),
                SearchOperations.MatchField(searchParams.SearchText, field: addressField, boost: 8),
                SearchOperations.MatchPhrasePrefix(searchParams.SearchText, field: addressField, boost: 8),
                SearchOperations.WildcardMatch(searchParams.SearchText, fields: new[] {addressField}, boost: 10),
            });
        }
        else if (indexName == "persons")
        {
            Fields nameFields = new[] { "title", "firstname", "surname" };
            Field tenureAddressField = "tenures.assetFullAddress";
            Fields tenureKeyFields = new[] {
                "tenures.id",
                "tenures.uprn",
                "tenures.propertyReference",
                "tenures.assetId",
                "tenures.paymentReference",
            };

            shouldOperations.AddRange(new[]
            {
                SearchOperations.MatchFields(searchParams.SearchText, fields: nameFields, boost: 12),
                SearchOperations.WildcardMatch(searchParams.SearchText, fields: nameFields, boost: 8),
                SearchOperations.Nested(
                    path: "tenures",
                    func: SearchOperations.MatchPhrasePrefix(searchParams.SearchText, field: tenureAddressField, boost: 12)
                ),
                SearchOperations.Nested(
                    path: "tenures",
                    func: SearchOperations.MatchField(searchParams.SearchText, field: tenureAddressField, boost: 10)
                ),
                SearchOperations.Nested(
                    path: "tenures",
                    func: SearchOperations.WildcardMatch(searchParams.SearchText, fields: new[] { tenureAddressField }, boost: 8)
                ),
                SearchOperations.Nested(
                    path: "tenures",
                    func: SearchOperations.MultiMatchBestFields(searchParams.SearchText, fields: tenureKeyFields, boost: 12)
                ),
            });
        }

        var searchResponse = await _elasticClient.SearchAsync<object>(s => s
            .Index(indexName)
            .Query(q => q
                .Bool(b => b
                    .Should(shouldOperations)
                )
            )
            .MinScore(0)
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


    // private static Func<QueryContainerDescriptor<object>, QueryContainer>
    //     MatchAddressField(string searchText, Field field, int boost) =>
    //     should => should
    //         .Bool(b => b
    //             .Should(
    //                 SearchOperations.MatchField(searchText, field, boost),
    //                 SearchOperations.MatchPhrasePrefix(searchText, field, boost),
    //                 SearchOperations.WildcardMatch(searchText, field, boost)
    //             )
    //         );


}
