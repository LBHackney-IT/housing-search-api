namespace HousingSearchApi.V1.Gateways.Models
{
    public class Identification
    {
        public string IdentificationType { get; set; }

        public string Value { get; set; }

        public bool OriginalDocumentSeen { get; set; }

        public string LinkToDocument { get; set; }

    }
}
