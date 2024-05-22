using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAllTenureListRequest : GetTenureListRequest
    {
        [FromQuery(Name = "lastHitId")]
        public string LastHitId { get; set; }
    }
}
