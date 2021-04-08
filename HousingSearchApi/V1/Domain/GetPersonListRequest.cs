using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Domain
{
    public class GetPersonListRequest
    {
        private const int DefaultPageSize = 12;

        [FromQuery(Name = "searchText")]
        [Required]
        [MinLength(2)]
        public string SearchText { get; set; }

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = DefaultPageSize;

        [FromQuery(Name = "page")]
        public int Page { get; set; }

        [FromQuery(Name = "sortBy")]
        public int SortBy { get; set; }

        [FromQuery(Name = "isDesc")]
        public bool IsDesc { get; set; }
    }
}
