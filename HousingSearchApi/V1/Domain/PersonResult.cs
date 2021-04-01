using System.Collections.Generic;
using System.Linq;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using Newtonsoft.Json;

namespace HousingSearchApi.V1.Domain
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
        public string Id { get; set; }

        public string Type { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string AssetFullAddress { get; set; }

    }
    public class Person
    {
        public static Person Create(QueryablePerson person)
        {
            return new Person
            {
                Id = person.Id,
                Title = person.Title,
                Firstname = person.Firstname,
                MiddleName = person.MiddleName,
                Surname = person.Surname,
                PreferredFirstname = person.PreferredFirstname,
                PreferredSurname = person.PreferredSurname,
                Ethinicity = person.Ethinicity,
                Nationality = person.Nationality,
                PlaceOfBirth = person.PlaceOfBirth,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                Identification = Create(person.Identification ?? new List<Infrastructure.Identification>()),
                PersonTypes = person.PersonTypes,
                IsPersonCautionaryAlert = person.IsPersonCautionaryAlert,
                IsTenureCautionaryAlert = person.IsTenureCautionaryAlert,
                Tenures = Create(person.Tenures ?? new List<Infrastructure.Tenures>())
            };
        }

        private static List<Identification> Create(List<Infrastructure.Identification> identifications)
        {
            var identList = new List<Identification>();

            foreach (Infrastructure.Identification identification in identifications)
            {
                identList.Add(new Identification
                {
                    IdentificationType = identification.IdentificationType,
                    LinkToDocument = identification.LinkToDocument,
                    OriginalDocumentSeen = identification.OriginalDocumentSeen,
                    Value = identification.Value
                });
            }

            return identList;
        }

        private static List<Tenures> Create(List<Infrastructure.Tenures> tenures)
        {
            var tenureList = new List<Tenures>();

            foreach (Infrastructure.Tenures tenure in tenures)
            {
                tenureList.Add(new Tenures
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

        public string Id { get; set; }

        public string Title { get; set; }

        public string Firstname { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public string PreferredFirstname { get; set; }

        public string PreferredSurname { get; set; }

        public string Ethinicity { get; set; }

        public string Nationality { get; set; }

        public string PlaceOfBirth { get; set; }

        public string DateOfBirth { get; set; }

        public string Gender { get; set; }

        public List<Identification> Identification { get; set; }

        public List<string> PersonTypes { get; set; }

        public bool IsPersonCautionaryAlert { get; set; }

        public bool IsTenureCautionaryAlert { get; set; }

        public List<Tenures> Tenures { get; set; }
    }
}
