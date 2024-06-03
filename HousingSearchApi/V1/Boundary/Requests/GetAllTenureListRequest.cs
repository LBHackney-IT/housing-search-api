using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAllTenureListRequest : GetTenureListRequest
    {
        [FromQuery(Name = "lastHitId")]
        public string LastHitId { get; set; }

        //in order to use sorting together with last hit id, this must be provided as well 
        [FromQuery(Name = "lastHitTenureStartDate")]
        public string LastHitTenureStartDate { get; set; }
    }
}
