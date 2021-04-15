namespace HousingSearchApi.V1.Boundary.Responses.Metadata
{
    /// <summary>
    /// API Response wrapper for all API responses
    /// If a request has been successful this will be denoted by the statusCode
    ///     Then the 'data' property will be populated
    /// If a request has not been successful denoted
    ///     Then the Error property will be populated
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResponse<T> where T : class
    {
        public T Results { get; set; }

        public long Total { get; set; }

        public APIResponse() { }

        public APIResponse(T result)
        {
            Results = result;
        }
    }
}
