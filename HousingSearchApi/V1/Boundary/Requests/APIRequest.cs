using HousingSearchApi.V1.Infrastructure;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class APIRequest
    {
        /// <summary>
        /// Text, which used for search
        /// </summary>
        /// <example>
        /// BlockName
        /// </example>
        public string SearchText { get; set; }

        /// <summary>
        /// Number of page size
        /// </summary>
        /// <example>
        /// 3
        /// </example>
        public int PageSize { get; set; } = Constants.DefaultPageSize;

        /// <summary>
        /// Number of current page
        /// </summary>
        /// <example>
        /// 1
        /// </example>
        public int PageNumber { get; set; }

        /// <summary>
        /// Field, by which we sort
        /// </summary>
        /// <example>
        /// AssetName
        /// </example>
        public string SortBy { get; set; }

        /// <summary>
        /// Sort descending or ascending
        /// </summary>
        /// <example>
        /// true
        /// </example>
        public bool IsDesc { get; set; }
    }
}
