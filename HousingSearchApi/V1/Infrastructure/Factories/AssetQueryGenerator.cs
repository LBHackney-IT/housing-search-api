using System;
using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;
using HousingSearchApi.V1.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Factories;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Factories
{
    public class AssetQueryGenerator : IQueryGenerator<QueryableAsset>
    {
        private readonly IQueryBuilder<QueryableAsset> _queryBuilder;
        private readonly IFilterQueryBuilder<QueryableAsset> _queryFilterBuilder;
        public AssetQueryGenerator(IQueryBuilder<QueryableAsset> queryBuilder,
            IFilterQueryBuilder<QueryableAsset> queryFilterBuilder)
        {
            _queryBuilder = queryBuilder;
            _queryFilterBuilder = queryFilterBuilder;
        }

        public QueryContainer Create<TRequest>(TRequest request, QueryContainerDescriptor<QueryableAsset> q)
        {
            //Pre-default check for relationships query
            if (request.GetType() == typeof(GetAssetRelationshipsRequest))
            {
                var relationshipRequest = request as GetAssetRelationshipsRequest;
                return _queryFilterBuilder
                    .WithExactQuery(relationshipRequest.SearchText, new List<string> { "parentAssetIds" })
                    .Build(q);
            }

            //Default to search endpoint
            GetAssetListRequest assetListRequest = request as GetAssetListRequest;

            if (assetListRequest == null)
                throw new ArgumentNullException($"{nameof(request).ToString()} shouldn't be null.");

            if (assetListRequest.IsSimpleQuery)
            {
                return _queryBuilder.BuildSimpleQuery(q, assetListRequest.SearchText, new List<string> { "assetAddress.addressLine1.textAddress", "assetAddress.postCode" });
            }

            if (request.GetType() == typeof(GetAssetListRequest))
            {
                //This is so assets search endpoint works as before
                return _queryFilterBuilder
                    .WithMultipleFilterQuery(assetListRequest.IsActive, new List<string> { "isActive" })
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
                GetAllAssetListRequest getAllAssetListRequest = request as GetAllAssetListRequest;

                if (assetListRequest.SearchText != null && assetListRequest.SearchText.Length > 0 && getAllAssetListRequest.IsTemporaryAccomodation == "true")
                {
                    // temp accom search 
                    return _queryFilterBuilder
                        .WithMultipleFilterQuery(getAllAssetListRequest.NumberOfBedrooms, new List<string>
                            {
                            "assetCharacteristics.numberOfBedrooms"
                            })
                        .WithMultipleFilterQuery(getAllAssetListRequest.NumberOfBedSpaces, new List<string> { "assetCharacteristics.numberOfBedSpaces" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.NumberOfCots, new List<string> { "assetCharacteristics.numberOfCots" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.FloorNo, new List<string> { "assetLocation.floorNo" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.PrivateBathroom, new List<string> { "assetCharacteristics.hasPrivateBathroom" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.PrivateKitchen, new List<string> { "assetCharacteristics.hasPrivateKitchen" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.StepFree, new List<string> { "assetCharacteristics.isStepFree" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.IsTemporaryAccomodation, new List<string> { "assetManagement.isTemporaryAccomodation" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.ParentAssetId, new List<string> { "rootAsset" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.IsActive, new List<string> { "isActive" })
                        .WithWildstarBoolQuery(getAllAssetListRequest.SearchText,
                            new List<string> { "assetAddress.addressLine1", "assetAddress.postCode", "assetAddress.uprn" })
                        .WithExactQuery(getAllAssetListRequest.SearchText,
                            new List<string>
                            {
                            "assetAddress.addressLine1",
                            "assetAddress.uprn",
                            "assetAddress.postCode"
                            })
                        .WithFilterQuery(getAllAssetListRequest.AssetTypes, new List<string> { "assetType" })
                        .WithFilterQuery(getAllAssetListRequest.AssetStatus, new List<string> { "assetManagement.propertyOccupiedStatus" })
                        .WithFilterQuery(getAllAssetListRequest.TenureType, new List<string> { "tenure.type.keyword" })

                        .Build(q);
                }
                if (assetListRequest.SearchText != null && assetListRequest.SearchText.Length > 0)
                {
                    //For when we need to use searchText and filters together                    
                    return _queryFilterBuilder
                        .WithMultipleFilterQuery(getAllAssetListRequest.NumberOfBedrooms, new List<string>
                            {
                            "assetCharacteristics.numberOfBedrooms"
                            })
                        .WithMultipleFilterQuery(getAllAssetListRequest.NumberOfBedSpaces, new List<string> { "assetCharacteristics.numberOfBedSpaces" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.NumberOfCots, new List<string> { "assetCharacteristics.numberOfCots" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.FloorNo, new List<string> { "assetLocation.floorNo" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.PrivateBathroom, new List<string> { "assetCharacteristics.hasPrivateBathroom" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.PrivateKitchen, new List<string> { "assetCharacteristics.hasPrivateKitchen" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.StepFree, new List<string> { "assetCharacteristics.isStepFree" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.IsTemporaryAccomodation, new List<string> { "assetManagement.isTemporaryAccomodation" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.ParentAssetId, new List<string> { "rootAsset" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.IsActive, new List<string> { "isActive" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.ContractIsApproved, new List<string> { "assetContract.isApproved" })
                        .WithWildstarQuery(getAllAssetListRequest.SearchText,
                            new List<string> { "assetAddress.addressLine1", "assetAddress.postCode", "assetAddress.uprn" })
                        .WithExactQuery(getAllAssetListRequest.SearchText,
                            new List<string>
                            {
                            "assetAddress.addressLine1",
                            "assetAddress.uprn",
                            "assetAddress.postCode"
                            })
                        .WithFilterQuery(getAllAssetListRequest.AssetTypes, new List<string> { "assetType" })
                        .WithFilterQuery(getAllAssetListRequest.AssetStatus, new List<string> { "assetManagement.propertyOccupiedStatus" })
                        .WithFilterQuery(getAllAssetListRequest.TenureType, new List<string> { "tenure.type.keyword" })

                        .Build(q);
                }
                else
                {
                    //Only the all enpoint should exclude need for SearchText                    
                    return _queryFilterBuilder
                        .WithMultipleFilterQuery(getAllAssetListRequest.NumberOfBedrooms, new List<string>
                            {
                            "assetCharacteristics.numberOfBedrooms"
                            })
                        .WithMultipleFilterQuery(getAllAssetListRequest.NumberOfBedSpaces, new List<string> { "assetCharacteristics.numberOfBedSpaces" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.NumberOfCots, new List<string> { "assetCharacteristics.numberOfCots" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.FloorNo, new List<string> { "assetLocation.floorNo" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.PrivateBathroom, new List<string> { "assetCharacteristics.hasPrivateBathroom" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.PrivateKitchen, new List<string> { "assetCharacteristics.hasPrivateKitchen" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.StepFree, new List<string> { "assetCharacteristics.isStepFree" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.IsTemporaryAccomodation, new List<string> { "assetManagement.isTemporaryAccomodation" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.ParentAssetId, new List<string> { "rootAsset" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.IsActive, new List<string> { "isActive" })
                        .WithMultipleFilterQuery(getAllAssetListRequest.ContractIsApproved, new List<string> { "assetContract.isApproved" })
                        .WithFilterQuery(getAllAssetListRequest.AssetTypes, new List<string> { "assetType" })
                        .WithFilterQuery(getAllAssetListRequest.AssetStatus, new List<string> { "assetManagement.propertyOccupiedStatus" })
                        .WithFilterQuery(getAllAssetListRequest.TenureType, new List<string> { "tenure.type.keyword" })
                        .Build(q);
                }
            }
        }
    }
}
