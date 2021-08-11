using HousingSearchApi.V1.Domain.Asset;
using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAssetListRequest
    {
        /// <summary>
        /// Text which used for search
        /// </summary>
        /// <example>
        /// BlockName
        /// </example>
        [FromQuery(Name = "searchText")]
        public string SearchText { get; set; }

        /// <summary>
        /// AssetType for which we are looking [Block, Estate]
        /// </summary>
        /// <example>
        /// Block
        /// </example>
        [FromQuery(Name = "assetType")]
        public AssetType AssetType { get; set; }

        /// <summary>
        /// Count objects on page
        /// </summary>
        /// <example>
        /// 3
        /// </example>
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 12;

        /// <summary>
        /// Number of current page
        /// </summary>
        /// <example>
        /// 1
        /// </example>
        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Field by which we sort
        /// </summary>
        /// <example>
        /// AssetName
        /// </example>
        [FromQuery(Name = "sortBy")]
        public string SortBy { get; set; }

        /// <summary>
        /// Sort descending or ascending
        /// </summary>
        /// <example>
        /// true
        /// </example>
        [FromQuery(Name = "isDesc")]
        public bool IsDesc { get; set; }
    }
}
