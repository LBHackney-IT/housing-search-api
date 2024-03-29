using Hackney.Shared.HousingSearch.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetPersonListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "personType")]
        public PersonType? PersonType { get; set; }
    }
}
