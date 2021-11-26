namespace HousingSearchApi.V1.Boundary.Responses.Metadata
{
    public class APIAllResponse<T> where T : class
    {
        public string LastHitId { get; set; }

        public T Results { get; set; }

        public long Total { get; set; }

        public APIAllResponse() { }

        public APIAllResponse(T result)
        {
            Results = result;
        }

    }
}
