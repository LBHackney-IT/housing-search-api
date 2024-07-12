using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAssetListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "assetTypes")]
        public string AssetTypes { get; set; }

        [FromQuery(Name = "numberOfBedrooms")]
        public string NumberOfBedrooms { get; set; }

        [FromQuery(Name = "isActive")]
        public string IsActive { get; set; }

        [FromQuery(Name = "useCustomSorting")]
        public bool UseCustomSorting { get; set; } = false;
    }
}
