using Nest;

namespace HousingSearchApi.V1.Gateways.Models
{
    public class QueryableIdentification
    {
        [Text(Name = "identificationType")]
        public string IdentificationType { get; set; }

        [Text(Name = "value")]
        public string Value { get; set; }

        [Text(Name = "originalDocumentSeen")]
        public bool OriginalDocumentSeen { get; set; }

        [Text(Name = "linkToDocument")]
        public string LinkToDocument { get; set; }
    }
}
