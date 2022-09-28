using Microsoft.AspNetCore.Mvc;
using System;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetProcessListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "targetType")]
        public string TargetType { get; set; }

        [FromQuery(Name = "targetId")]
        public Guid? TargetId { get; set; }

        [FromQuery(Name = "isOpen")]
        public bool IsOpen { get; set; }

    }
}
