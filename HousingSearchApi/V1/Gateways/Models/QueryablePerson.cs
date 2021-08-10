using HousingSearchApi.V1.Domain.Person;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace HousingSearchApi.V1.Gateways.Models
{
    public class QueryablePerson
    {
        public Person Create()
        {
            var listOfIdentifications = Identification != null ? Identification.Select(x => Domain.Person.Identification.Create(x?.IdentificationType,
                x?.Value, x.OriginalDocumentSeen, x?.LinkToDocument)).ToList() : new List<Domain.Person.Identification>();

            var listOfTenures = Tenures == null ? new List<Tenure>() :
                Tenures.Select(x => Tenure.Create(x?.Id, x?.Type, x.TotalBalance, x?.StartDate, x?.EndDate, x?.AssetFullAddress, x?.PostCode, x?.RentAccountNumber)).ToList();

            var listOfProperties = Properties == null ? new List<PersonProperty>() :
                Properties.Select(x => PersonProperty.Create(x?.Id, x?.Type, x.TotalBalance, x?.AssetFullAddress, x?.PostCode, x?.RentAccountNumber)).ToList();

            return Person.Create(Id, Title, Firstname, MiddleName, Surname, PreferredFirstname,
                PreferredSurname, TotalBalance, Ethinicity, Nationality, PlaceOfBirth, DateOfBirth, Gender, listOfIdentifications,
                PersonTypes, IsPersonCautionaryAlert, IsTenureCautionaryAlert, listOfTenures, listOfProperties);
        }

        [Text(Name = "id")]
        public string Id { get; set; }

        [Text(Name = "title")]
        public string Title { get; set; }

        [Keyword(Name = "firstname")]
        public string Firstname { get; set; }

        [Text(Name = "middleName")]
        public string MiddleName { get; set; }

        [Keyword(Name = "surname")]
        public string Surname { get; set; }

        [Text(Name = "preferredFirstname")]
        public string PreferredFirstname { get; set; }

        [Text(Name = "preferredSurname")]
        public string PreferredSurname { get; set; }

        [Text(Name = "totalBalance")]
        public decimal TotalBalance { get; set; }

        [Text(Name = "ethinicity")]
        public string Ethinicity { get; set; }

        [Text(Name = "nationality")]
        public string Nationality { get; set; }

        [Text(Name = "placeOfBirth")]
        public string PlaceOfBirth { get; set; }

        [Text(Name = "dateOfBirth")]
        public string DateOfBirth { get; set; }

        [Text(Name = "gender")]
        public string Gender { get; set; }

        [Text(Name = "identification")]
        public List<QueryableIdentification> Identification { get; set; }

        [Text(Name = "personTypes")]
        public List<string> PersonTypes { get; set; }

        [Text(Name = "isPersonCautionaryAlert")]
        public bool IsPersonCautionaryAlert { get; set; }

        [Text(Name = "isTenureCautionaryAlert")]
        public bool IsTenureCautionaryAlert { get; set; }

        [Text(Name = "tenures")]
        public List<QueryablePersonTenure> Tenures { get; set; }

        [Text(Name = "properties")]
        public List<QueryablePersonProperty> Properties { get; set; }
    }
}
