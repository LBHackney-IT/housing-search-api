using Newtonsoft.Json;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetAllTenureListResponse : GetTenureListResponse
    {
        [JsonIgnore]
        public string LastHitTenureStartDate { get; set; }

        [JsonIgnore]
        public string LastHitId { get; set; }
    }
}
