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

    const int HighBoost = 3;
    const int MediumBoost = 2;
    const int LowBoost = 1;

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
                SearchOperations.MultiMatchBestFields(searchParams.SearchText, fields: keywordFields, boost: HighBoost),
                SearchOperations.MultiMatchCrossFields(searchParams.SearchText, fields: addressFieldNames, boost: LowBoost),
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
                SearchOperations.MatchField(searchParams.SearchText, field: nameField, boost: MediumBoost),
                SearchOperations.WildcardMatch(searchParams.SearchText, fields: new [] {nameField}, boost: LowBoost),
                SearchOperations.MultiMatchBestFields(searchParams.SearchText, fields: keywordFields, boost: MediumBoost),
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
                SearchOperations.MatchFields(searchParams.SearchText, fields: nameFields, boost: HighBoost),
                SearchOperations.WildcardMatch(searchParams.SearchText, fields: nameFields, boost: LowBoost),
                SearchOperations.Nested(
                    path: "tenures",
                    func: SearchOperations.MultiMatchBestFields(searchParams.SearchText, fields: tenureKeyFields, boost: HighBoost)
                ),
                SearchOperations.Nested(
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

    private Func<QueryContainerDescriptor<object>, QueryContainer>
        MatchAddressField(string searchText, Field field) =>
        should => should
            .Bool(b => b
                .Should(
                    SearchOperations.MatchPhrasePrefix(searchText, field, boost: HighBoost),
                    SearchOperations.MatchField(searchText, field, boost: MediumBoost),
                    SearchOperations.QueryStringQuery(searchText, new List<string> { field.ToString() }, boost: HighBoost)
                )
            );


}
