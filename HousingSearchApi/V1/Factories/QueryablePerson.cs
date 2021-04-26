using System.Collections.Generic;
using HousingSearchApi.V1.Domain;
using Nest;
using Identification = HousingSearchApi.V1.Domain.ElasticSearch.Identification;
using Tenures = HousingSearchApi.V1.Domain.ElasticSearch.Tenures;

namespace HousingSearchApi.V1.Factories
{
    public class QueryablePerson
    {
        public Person Create()
        {
            return new Person
            {
                Id = Id,
                Title = Title,
                Firstname = Firstname,
                MiddleName = MiddleName,
                Surname = Surname,
                PreferredFirstname = PreferredFirstname,
                PreferredSurname = PreferredSurname,
                Ethinicity = Ethinicity,
                Nationality = Nationality,
                PlaceOfBirth = PlaceOfBirth,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                Identification = Create(Identification ?? new List<Identification>()),
                PersonTypes = PersonTypes,
                IsPersonCautionaryAlert = IsPersonCautionaryAlert,
                IsTenureCautionaryAlert = IsTenureCautionaryAlert,
                Tenures = Create(Tenures ?? new List<Tenures>())
            };
        }

        private static List<Domain.Identification> Create(List<Identification> identifications)
        {
            var identList = new List<Domain.Identification>();

            foreach (Identification identification in identifications)
            {
                identList.Add(new Domain.Identification
                {
                    IdentificationType = identification.IdentificationType,
                    LinkToDocument = identification.LinkToDocument,
                    OriginalDocumentSeen = identification.OriginalDocumentSeen,
                    Value = identification.Value
                });
            }

            return identList;
        }

        private static List<Domain.Tenures> Create(List<Tenures> tenures)
        {
            var tenureList = new List<Domain.Tenures>();

            foreach (Tenures tenure in tenures)
            {
                tenureList.Add(new Domain.Tenures
                {
                    AssetFullAddress = tenure.AssetFullAddress,
                    EndDate = tenure.EndDate,
                    Id = tenure.Id,
                    StartDate = tenure.StartDate,
                    Type = tenure.Type
                });
            }

            return tenureList;
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

        public List<Identification> Identification { get; set; }

        public List<string> PersonTypes { get; set; }

        public bool IsPersonCautionaryAlert { get; set; }

        public bool IsTenureCautionaryAlert { get; set; }

        public List<Tenures> Tenures { get; set; }

    }
}
