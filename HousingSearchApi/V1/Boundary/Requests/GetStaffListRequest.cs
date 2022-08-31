using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetStaffListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "emailAddress")]
        public string EmailAddress { get; set; }
    }
}
