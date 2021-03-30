using System.Collections.Generic;
using Nest;

namespace HousingSearchApi.V1.Infrastructure
{
    public class Identification
    {
        public string IdentificationType { get; set; }

        public string Value { get; set; }

        public bool OriginalDocumentSeen { get; set; }

        public string LinkToDocument { get; set; }

    }
    public class Tenures
    {
        [Text(Name = "id")]
        public string Id { get; set; }

        [Text(Name = "type")]
        public string Type { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string AssetFullAddress { get; set; }

    }
    public class QueryablePerson
    {
        [Text(Name = "id")]
        public string Id { get; set; }
        public string Title { get; set; }

        [Text(Name = "firstname")]
        public string Firstname { get; set; }

        [Text(Name = "middlename")]
        public string MiddleName { get; set; }

        [Text(Name = "surname")]
        public string Surname { get; set; }

        [Text(Name = "preferredFirstname")]
        public string PreferredFirstname { get; set; }

        [Text(Name = "preferredSurname")]
        public string PreferredSurname { get; set; }

        public string Ethinicity { get; set; }

        public string Nationality { get; set; }

        public string PlaceOfBirth { get; set; }

        [Text(Name = "dateOfBirth")]
        public string DateOfBirth { get; set; }

        public string Gender { get; set; }

        public List<Identification> Identification { get; set; }

        public List<string> PersonTypes { get; set; }

        public bool IsPersonCautionaryAlert { get; set; }

        public bool IsTenureCautionaryAlert { get; set; }

        public List<Tenures> Tenures { get; set; }

    }
}
