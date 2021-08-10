using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetPersonListRequest
    {
        [FromQuery(Name = "personType")]
        public PersonType PersonType { get; set; }

        [FromQuery(Name = "searchText")]
        public string SearchText { get; set; }

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = Constants.DefaultPageSize;

        [FromQuery(Name = "page")]
        public int Page { get; set; }

        [FromQuery(Name = "sortBy")]
        public string SortBy { get; set; }

        [FromQuery(Name = "isDesc")]
        public bool IsDesc { get; set; }
    }
}
