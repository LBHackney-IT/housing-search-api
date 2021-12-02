using System;

namespace HousingSearchApi.V1.Boundary.Requests.Interfaces
{
    interface ISearchByTargetId
    {
        public Guid TargetId { get; set; }
    }
}
