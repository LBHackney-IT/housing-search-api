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
        var shouldOperations = new List<Func<QueryContainerDescriptor<object>, QueryContainer>>() { };

        // Extend search operations depending on the index
        if (indexName == "assets")
        {
            Fields addressFieldNames = new[] { "assetAddress.addressLine1", "assetAddress.addressLine2", "assetAddress.uprn", "assetAddress.postCode" };
            Fields tenureFields = new[] { "tenure.id", "tenure.paymentReference" };
            shouldOperations.AddRange(new[]
            {
                SearchOperations.MultiMatchBestFields(searchParams.SearchText, boost: 6),
                SearchOperations.MultiMatchCrossFields(searchParams.SearchText, fields: addressFieldNames, boost: 10),
                SearchOperations.WildcardMatch(searchParams.SearchText, fieldNames: new[] {"assetAddress.addressLine1"}, boost: 10),
                SearchOperations.MultiMatchBestFields(searchParams.SearchText, fields: tenureFields, boost: 10),
            });
        }
        else if (indexName == "tenures")
        {
            Fields nameFields = new[] { "householdMembers.fullName" };
            Fields keyFields = new[] {
                "id", "paymentReference", // Main fields
                "tenuredAsset.id", "tenuredAsset.fullAddress", "tenuredAsset.uprn", // Address fields
            };
            shouldOperations.AddRange(new[]
            {
                SearchOperations.MultiMatchBestFields(searchParams.SearchText, boost: 6),
                SearchOperations.WildcardMatch(searchParams.SearchText, fieldNames: nameFields, boost: 10),
                SearchOperations.MultiMatchCrossFields(searchParams.SearchText, fields: keyFields, boost: 10),
            });
        }
        else if (indexName == "persons")
        {
            Fields nameFields = new[] { "title", "firstname", "surname" };
            Fields addressFields = new[] {
                "tenures.id",
                "tenures.assetFullAddress",
                "tenures.type",
                "tenures.uprn",
                "tenures.propertyReference",
                "tenures.assetId",
                "tenures.paymentReference",
            };

            shouldOperations.AddRange(new[]
            {
                SearchOperations.MultiMatchMostFields(searchParams.SearchText, boost: 10, fields: nameFields),
                SearchOperations.NestedMultiMatch(searchParams.SearchText, "tenures", addressFields, boost: 10),
            });
        }
        else
        {
            shouldOperations.AddRange(
                new[] {
                    SearchOperations.MultiMatchBestFields(searchParams.SearchText, boost: 6)
                }
            );
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
}
