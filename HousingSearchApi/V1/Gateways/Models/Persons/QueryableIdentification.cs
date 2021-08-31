namespace HousingSearchApi.V1.Gateways.Models.Persons
{
    public class QueryableIdentification
    {
        public QueryableIdentification()
        {

        }

        public static QueryableIdentification Create(string identificationType, string value, bool originalDocumentSeen,
            string linkToDocument)
        {
            return new QueryableIdentification(identificationType, value, originalDocumentSeen, linkToDocument);
        }

        private QueryableIdentification(string identificationType, string value, bool originalDocumentSeen, string linkToDocument)
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
