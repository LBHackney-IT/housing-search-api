using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAssetListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "assetTypes")]
        public string AssetTypes { get; set; }

        [FromQuery(Name = "numberOfBedrooms ")]
        public string NumberOfBedrooms { get; set; }
    }
}
