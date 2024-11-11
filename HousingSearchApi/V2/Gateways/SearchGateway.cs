using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HousingSearchApi.V2.Domain.DTOs;
using HousingSearchApi.V2.Gateways.Interfaces;
using Nest;
using Ops = HousingSearchApi.V2.Gateways.SearchOperations;

namespace HousingSearchApi.V2.Gateways;


public class SearchGateway : ISearchGateway
{
    private readonly IElasticClient _elasticClient;

    private static class BoostLevel
    {
        public const int High = 3;
        public const int Medium = 2;
        public const int Low = 1;
    }

    public SearchGateway(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<SearchResponseDto> Search(string indexName, SearchParametersDto searchParams)
    {

        var shouldOperations = new List<Func<QueryContainerDescriptor<object>, QueryContainer>>() {
            Ops.MultiMatchBestFields(searchParams.SearchText, boost: 6),
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
                Ops.MultiMatchBestFields(searchParams.SearchText, fields: keywordFields, boost: BoostLevel.High),
                Ops.MultiMatchCrossFields(searchParams.SearchText, fields: addressFieldNames, boost: BoostLevel.Low),
                MatchAddressField(searchParams.SearchText, keyAddressField),
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
                MatchNameFields(searchParams.SearchText, new[] {nameField}),
                Ops.MultiMatchBestFields(searchParams.SearchText, fields: keywordFields, boost: BoostLevel.Medium),
                MatchAddressField(searchParams.SearchText, addressField),
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
                MatchNameFields(searchParams.SearchText, nameFields),
                Ops.Nested(
                    path: "tenures",
                    func: Ops.MultiMatchBestFields(searchParams.SearchText, fields: tenureKeyFields, boost: BoostLevel.High)
                ),
                Ops.Nested(
                    path: "tenures",
                    func: MatchAddressField(searchParams.SearchText, tenureAddressField)
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
            .MinScore(5)
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


    static Func<QueryContainerDescriptor<object>, QueryContainer>
        MatchNameFields(string searchText, Fields fields) =>
        should => should
            .Bool(b => b
                .Should(
                    Ops.MatchFields(searchText, fields, boost: BoostLevel.High),
                    Ops.WildcardMatch(searchText, fields, boost: BoostLevel.Low)
                )
            );


    static Func<QueryContainerDescriptor<object>, QueryContainer>
        MatchAddressField(string searchText, Field field) =>
        should => should
            .Bool(b => b
                .Should(
                    Ops.MatchPhrasePrefix(searchText, field, boost: BoostLevel.High),
                    Ops.MatchField(searchText, field, boost: BoostLevel.Medium),
                    Ops.WildcardQueryStringQuery(searchText, new[] { field }, boost: BoostLevel.High)
                )
            );


}
