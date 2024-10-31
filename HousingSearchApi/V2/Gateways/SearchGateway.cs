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
        var shouldOperations = new List<Func<QueryContainerDescriptor<object>, QueryContainer>>();

        // General purpose schema-agnostic search operations
        var defaultShouldOperations = new[]
        {
            SearchOperations.MultiMatchSingleField(searchParams.SearchText, boost: 6)
        };

        // Extend search operations depending on the index
        if (indexName == "assets")
        {
            var addressFieldName = "assetAddress.addressLine1";
            Fields tenureFields = new[] { "tenure.id", "tenure.paymentReference" };
            shouldOperations.AddRange(new[]
            {
                SearchOperations.MatchPhrasePrefix(searchParams.SearchText, fieldName: addressFieldName, boost: 10),
                SearchOperations.WildcardMatch(searchParams.SearchText, fieldName: addressFieldName, boost: 5),
                SearchOperations.NestedMultiMatch(searchParams.SearchText, "tenure", tenureFields, boost: 5),
            });
            shouldOperations.AddRange(defaultShouldOperations);
        }
        else if (indexName == "tenures")
        {
            var addressFieldName = "tenuredAsset.fullAddress";
            var nameFields = new List<string> { "householdMembers.fullName" };
            shouldOperations.AddRange(new[]
            {
                SearchOperations.SearchWithWildcardQuery(searchParams.SearchText, fields: nameFields, boost: 10),
                SearchOperations.MatchPhrasePrefix(searchParams.SearchText, fieldName: addressFieldName, boost: 10),
                SearchOperations.WildcardMatch(searchParams.SearchText, fieldName: addressFieldName, boost: 5),
            });
            shouldOperations.AddRange(defaultShouldOperations);
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
                SearchOperations.MultiMatchSingleField(searchParams.SearchText, boost: 6),
                SearchOperations.MultiMatchMostFields(searchParams.SearchText, boost: 10, fields: nameFields),
                SearchOperations.NestedMultiMatch(searchParams.SearchText, "tenures", addressFields, boost: 10),
            });
        }
        else
        {
            shouldOperations.AddRange(defaultShouldOperations);
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
