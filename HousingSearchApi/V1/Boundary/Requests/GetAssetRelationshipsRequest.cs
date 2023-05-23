using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAssetRelationshipsRequest : HousingSearchRequest
    {
        [FromQuery(Name = "treeLayers")]
        public int? TreeDepth { get; set; } = 3;
    }
}
