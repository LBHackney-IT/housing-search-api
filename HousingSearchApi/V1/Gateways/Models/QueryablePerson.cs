using System.Collections.Generic;
using System.Linq;
using HousingSearchApi.V1.Domain;
using Nest;

namespace HousingSearchApi.V1.Gateways.Models
{
    public class QueryablePerson
    {
        public Person Create()
        {
            var listOfIdentifications = Identification != null ? Identification.Select(x => Domain.Identification.Create(x?.IdentificationType,
                x?.Value, x.OriginalDocumentSeen, x?.LinkToDocument)).ToList() : new List<Domain.Identification>();
            var listOfTenures = Tenures != null ?
                Tenures.Select(x => Tenure.Create(x?.Id, x?.Type, x?.StartDate, x?.EndDate, x?.AssetFullAddress)).ToList() : new List<Tenure>();

            return Person.Create(Id, Title, Firstname, MiddleName, Surname, PreferredFirstname,
                PreferredSurname, Ethinicity, Nationality, PlaceOfBirth, DateOfBirth, Gender, listOfIdentifications,
                PersonTypes,
                IsPersonCautionaryAlert, IsTenureCautionaryAlert, listOfTenures);
        }

        [Text(Name = "id")]
        public string Id { get; set; }
        public string Title { get; set; }

        [Keyword(Name = "firstname")]
        public string Firstname { get; set; }

        [Text(Name = "middlename")]
        public string MiddleName { get; set; }

        [Keyword(Name = "surname")]
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

        public List<Domain.Identification> Identification { get; set; }

        public List<string> PersonTypes { get; set; }

        public bool IsPersonCautionaryAlert { get; set; }

        public bool IsTenureCautionaryAlert { get; set; }

        public List<Tenures> Tenures { get; set; }

    }
}
