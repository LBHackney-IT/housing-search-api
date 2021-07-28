namespace HousingSearchApi.V1.Domain
{
    public class Identification
    {
        public Identification() { }

        public static Identification Create(string identificationType, string value, bool originalDocumentSeen,
            string linkToDocument)
        {
            return new Identification(identificationType, value, originalDocumentSeen, linkToDocument);
        }

        private Identification(string identificationType, string value, bool originalDocumentSeen, string linkToDocument)
        {
            IdentificationType = identificationType;
            Value = value;
            OriginalDocumentSeen = originalDocumentSeen;
            LinkToDocument = linkToDocument;
        }

        public string IdentificationType { get; set; }

        public string Value { get; set; }

        public bool OriginalDocumentSeen { get; set; }

        public string LinkToDocument { get; set; }

    }
}
