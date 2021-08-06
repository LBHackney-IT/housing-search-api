namespace HousingSearchApi.V1.Boundary.Requests
{
    public class APIRequest
    {
        private const int DefaultPageSize = 12;

        public string SearchText { get; set; }

        public int PageSize { get; set; } = DefaultPageSize;

        public int PageNumber { get; set; }

        public string SortBy { get; set; }

        public bool IsDesc { get; set; }
    }
}
