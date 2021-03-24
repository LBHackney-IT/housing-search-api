using Newtonsoft.Json;

namespace HousingSearchApi.V1.Domain
{
    public class Tenure
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("assetFullAddress")]
        public string AssetFullAddress { get; set; }
    }
}
