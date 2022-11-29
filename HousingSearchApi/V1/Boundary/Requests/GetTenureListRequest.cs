using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetTenureListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "uprn")]
        public string Uprn { get; set; }
    }
}
