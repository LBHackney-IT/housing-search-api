namespace HousingSearchApi.V1.Domain
{
    public class Identification
    {
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

        public string IdentificationType { get; }

        public string Value { get; }

        public bool OriginalDocumentSeen { get; }

        public string LinkToDocument { get; }

    }
}
