using System;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAccountListRequest : HousingSearchRequest
    {
        public Guid TargetId { get; set; }
    }
}
