using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAllAssetListRequest : GetAssetListRequest
    {
        [FromQuery(Name = "lastHitId")]
        public string LastHitId { get; set; }

        [FromQuery(Name = "assetStatus")]
        public string AssetStatus { get; set; }

        [FromQuery(Name = "numberOfBedSpaces")]
        public string NumberOfBedSpaces { get; set; }

        [FromQuery(Name = "numberOfCots")]
        public string NumberOfCots { get; set; }

        [FromQuery(Name = "floorNo")]
        public string FloorNo { get; set; }

        [FromQuery(Name = "privateBathroom")]
        public string PrivateBathroom { get; set; }

        [FromQuery(Name = "privateKitchen")]
        public string PrivateKitchen { get; set; }

        [FromQuery(Name = "stepFree")]
        public string StepFree { get; set; }

        [FromQuery(Name = "contractIsApproved")]
        public string ContractIsApproved { get; set; }

        [FromQuery(Name = "parentAssetId")]
        public string ParentAssetId { get; set; }

        [FromQuery(Name = "tenureType")]
        public string TenureType { get; set; }
    }
}
