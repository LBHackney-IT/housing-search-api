using HousingSearchApi.V1.Infrastructure.Sorting.Enum;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAccountListRequest
    {
        private const int DefaultPageSize = 12;

        [FromQuery(Name = "searchText")]
        public string SearchText { get; set; }

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = DefaultPageSize;

        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "sortBy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EAccountSortBy SortBy { get; set; }

        [FromQuery(Name = "isDesc")]
        public bool IsDesc { get; set; }
    }
}
