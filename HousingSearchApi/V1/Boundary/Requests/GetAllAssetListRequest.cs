using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAllAssetListRequest : GetAssetListRequest
    {
        [FromQuery(Name = "lastHitId")]
        public string LastHitId { get; set; }
    }
}
