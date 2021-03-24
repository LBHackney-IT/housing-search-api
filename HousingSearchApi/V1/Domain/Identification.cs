using Newtonsoft.Json;

namespace SearchApi.V1.Domain
{
    public class Identification
    {
        [JsonProperty("identificationType")]
        public string IdentificationType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("isOriginalDocumentSeen")]
        public bool IsOriginalDocumentSeen { get; set; }

        [JsonProperty("linkToDocument")]
        public string LinkToDocument { get; set; }
    }
}
