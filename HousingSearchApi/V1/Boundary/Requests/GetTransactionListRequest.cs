using Microsoft.AspNetCore.Mvc;
using System;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetTransactionListRequest : HousingSearchRequest
    {
        /// <summary>
        /// Date by which filtering begins
        /// </summary>
        /// <example>
        /// 2020-04-27
        /// </example>
        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Date by which filtering ends
        /// </summary>
        /// <example>
        /// 2021-12-01
        /// </example>
        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }
    }
}
