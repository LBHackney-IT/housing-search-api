using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetStaffListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "email")]
        public string EmailAddress { get; set; }
    }
}
