using System.Collections.Generic;

namespace HousingSearchApi.V1.Domain.Person
{
    public class Person
    {
        public Person() { }

        public static Person Create(string id, string title, string firstname, string middleName, string surname,
            string preferredFirstname, string preferredSurname, decimal totalBalance, string ethinicity, string nationality,
            string placeOfBirth, string dateOfBirth, string gender,
            List<Identification> identifications,
            List<string> personTypes, bool isPersonCautionaryAlert, bool isTenureCautionaryAlert,
            List<PersonTenure> tenures)
        {
            return new Person(id, title, firstname, middleName, surname,
                preferredFirstname, preferredSurname, totalBalance, ethinicity, nationality,
                placeOfBirth, dateOfBirth, gender,
                identifications,
                personTypes, isPersonCautionaryAlert, isTenureCautionaryAlert,
                tenures);
        }

        private Person(string id, string title, string firstname, string middleName, string surname,
            string preferredFirstname, string preferredSurname, decimal totalBalance, string ethinicity, string nationality, string placeOfBirth, string dateOfBirth, string gender,
            List<Identification> identification,
            List<string> personTypes, bool isPersonCautionaryAlert, bool isTenureCautionaryAlert,
            List<PersonTenure> tenures)
        {
            Id = id;
            Title = title;
            Firstname = firstname;
            MiddleName = middleName;
            Surname = surname;
            PreferredFirstname = preferredFirstname;
            PreferredSurname = preferredSurname;
            TotalBalance = totalBalance;
            Ethinicity = ethinicity;
            Nationality = nationality;
            PlaceOfBirth = placeOfBirth;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Identification = identification;
            PersonTypes = personTypes;
            IsPersonCautionaryAlert = isPersonCautionaryAlert;
            IsTenureCautionaryAlert = isTenureCautionaryAlert;
            Tenures = tenures;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Firstname { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public string PreferredFirstname { get; set; }

        public string PreferredSurname { get; set; }

        public decimal TotalBalance { get; set; }

        public string Ethinicity { get; set; }

        public string Nationality { get; set; }

        public string PlaceOfBirth { get; set; }

        public string DateOfBirth { get; set; }

        public string Gender { get; set; }

        public List<Identification> Identification { get; set; }

        public List<string> PersonTypes { get; set; }

        public bool IsPersonCautionaryAlert { get; set; }

        public bool IsTenureCautionaryAlert { get; set; }

        public List<PersonTenure> Tenures { get; set; }
    }
}
