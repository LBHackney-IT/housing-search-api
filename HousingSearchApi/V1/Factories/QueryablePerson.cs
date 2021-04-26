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
                Id = this.Id,
                Title = this.Title,
                Firstname = this.Firstname,
                MiddleName = this.MiddleName,
                Surname = this.Surname,
                PreferredFirstname = this.PreferredFirstname,
                PreferredSurname = this.PreferredSurname,
                Ethinicity = this.Ethinicity,
                Nationality = this.Nationality,
                PlaceOfBirth = this.PlaceOfBirth,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender,
                Identification = Create(this.Identification ?? new List<Domain.ElasticSearch.Identification>()),
                PersonTypes = this.PersonTypes,
                IsPersonCautionaryAlert = this.IsPersonCautionaryAlert,
                IsTenureCautionaryAlert = this.IsTenureCautionaryAlert,
                Tenures = Create(this.Tenures ?? new List<Domain.ElasticSearch.Tenures>())
            };
        }

        private static List<Domain.Identification> Create(List<Domain.ElasticSearch.Identification> identifications)
        {
            var identList = new List<Domain.Identification>();

            foreach (Domain.ElasticSearch.Identification identification in identifications)
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

        private static List<Domain.Tenures> Create(List<Domain.ElasticSearch.Tenures> tenures)
        {
            var tenureList = new List<Domain.Tenures>();

            foreach (Domain.ElasticSearch.Tenures tenure in tenures)
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
