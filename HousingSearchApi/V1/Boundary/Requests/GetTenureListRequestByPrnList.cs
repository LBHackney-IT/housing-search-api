using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetTenureListRequestByPrnList
    {
        private const int DefaultPageSize = 12;
        /// <summary>
        /// Payment reference list. Can be empty to return all transactions
        /// </summary>
        /// <example>HSGSUN</example>
        [FromQuery(Name = "prnList")]
        public List<string> PrnList { get; set; }

        /// <summary>
        /// Page size. Default value is 12
        /// </summary>
        /// <example>10</example>
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = DefaultPageSize;

        /// <summary>
        /// Page number for pagination
        /// </summary>
        /// <example>1</example>
        [FromQuery(Name = "page")]
        public int Page { get; set; }

        [FromQuery(Name = "sortBy")]
        public string SortBy { get; set; }

        [FromQuery(Name = "isDesc")]
        public bool IsDesc { get; set; }

    }
}
