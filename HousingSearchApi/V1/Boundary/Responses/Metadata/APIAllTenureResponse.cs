namespace HousingSearchApi.V1.Boundary.Responses.Metadata
{
    public class APIAllTenureResponse<T> : APIAllResponse<T> where T : class
    {
        public string LastHitTenureStartDate { get; set; }
    }
}
