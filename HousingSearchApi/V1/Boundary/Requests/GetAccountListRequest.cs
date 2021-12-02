using System;
using HousingSearchApi.V1.Boundary.Requests.Interfaces;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAccountListRequest : HousingSearchRequest,ISearchByTargetId
    {
        public Guid TargetId { get; set; }
    }
}
