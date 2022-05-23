using System;
using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Factories;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Factories
{
    public class AssetQueryGenerator : IQueryGenerator<QueryableAsset>
    {
        private readonly IQueryBuilder<QueryableAsset> _queryBuilder;

        public AssetQueryGenerator(IQueryBuilder<QueryableAsset> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public QueryContainer Create<TRequest>(TRequest request, QueryContainerDescriptor<QueryableAsset> q)
        {
            //Default to search endpoint
            GetAssetListRequest assetListRequest = request as GetAssetListRequest;

            if (assetListRequest == null)
                throw new ArgumentNullException($"{nameof(request).ToString()} shouldn't be null.");

            if (request.GetType() == typeof(GetAssetListRequest))
            {
                //This is so assets search endpoint works as before
                return _queryBuilder
                    .WithWildstarQuery(assetListRequest.SearchText,
                        new List<string> { "assetAddress.addressLine1", "assetAddress.postCode", "assetAddress.uprn" })
                    .WithExactQuery(assetListRequest.SearchText,
                        new List<string>
                        {
                            "assetAddress.addressLine1",
                            "assetAddress.uprn",
                            "assetAddress.postCode"
                        })
                    .WithFilterQuery(assetListRequest.AssetTypes, new List<string> { "assetType" })
                    .Build(q);
            }
            else
            {
                if (assetListRequest.SearchText != null && assetListRequest.SearchText.Length > 0)
                {
                    return _queryBuilder
                        .WithWildstarQuery(assetListRequest.SearchText,
                            new List<string> { "assetAddress.addressLine1", "assetAddress.postCode", "assetAddress.uprn" })
                        .WithExactQuery(assetListRequest.SearchText,
                            new List<string>
                            {
                            "assetAddress.addressLine1",
                            "assetAddress.uprn",
                            "assetAddress.postCode"
                            })
                        .WithFilterQuery(assetListRequest.AssetTypes, new List<string> { "assetType" })
                        .Build(q);
                }
                else
                {
                    //Only the all enpoint should exclude need for SearchText
                    GetAllAssetListRequest assetListAllRequest = request as GetAllAssetListRequest;
                    return _queryBuilder
                        .WithFilterQuery(assetListAllRequest.AssetTypes, new List<string> { "assetType" })
                        .WithFilterQuery(assetListAllRequest.AssetStatus, new List<string> { "assetStatus" })
                        .WithFilterQuery(assetListAllRequest.NumberOfBedrooms, new List<string>
                            {
                            "assetCharacteristics.numberOfBedrooms"
                            })
                        .WithFilterQuery(assetListAllRequest.NumberOfBedSpaces, new List<string> { "numberOfBedSpaces" })
                        .WithFilterQuery(assetListAllRequest.NumberOfCots, new List<string> { "numberOfCots" })
                        .WithFilterQuery(assetListAllRequest.GroundFloor, new List<string> { "groundFloor" })
                        .WithFilterQuery(assetListAllRequest.PrivateBathroom, new List<string> { "privateBathroom" })
                        .WithFilterQuery(assetListAllRequest.PrivateKitchen, new List<string> { "privateKitchen" })
                        .WithFilterQuery(assetListAllRequest.StepFree, new List<string> { "stepFree" })
                        .WithFilterQuery(assetListAllRequest.IsTemporaryAccomodation, new List<string> { "isTemporaryAccomodation" })
                        .WithFilterQuery(assetListAllRequest.ParentAssetId, new List<string> { "parentAssetId" })
                        .Build(q);
                }
            }            
        }
    }
}
