using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetTenureListByPrnListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "prnList")]
        public List<string> PrnList { get; set; }
    }
}
