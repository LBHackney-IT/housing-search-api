using System.Collections.Generic;

namespace HousingSearchApi.V1.Domain
{
    public class Person
    {
        public Person()
        {

        }

        public static Person Create(string id, string title, string firstname, string middleName, string surname,
            string preferredFirstname, string preferredSurname, string ethinicity, string nationality,
            string placeOfBirth, string dateOfBirth, string gender,
            List<Identification> identifications,
            List<string> personTypes, bool isPersonCautionaryAlert, bool isTenureCautionaryAlert,
            List<Tenure> tenures)
        {
            return new Person(id, title, firstname, middleName, surname,
                preferredFirstname, preferredSurname, ethinicity, nationality,
                placeOfBirth, dateOfBirth, gender,
                identifications,
                personTypes, isPersonCautionaryAlert, isTenureCautionaryAlert,
                tenures);
        }

        private Person(string id, string title, string firstname, string middleName, string surname,
            string preferredFirstname, string preferredSurname, string ethinicity, string nationality, string placeOfBirth, string dateOfBirth, string gender,
            List<Identification> identification,
            List<string> personTypes, bool isPersonCautionaryAlert, bool isTenureCautionaryAlert,
            List<Tenure> tenures)
        {
            Id = id;
            Title = title;
            Firstname = firstname;
            MiddleName = middleName;
            Surname = surname;
            PreferredFirstname = preferredFirstname;
            PreferredSurname = preferredSurname;
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

        public string Id { get; }

        public string Title { get; }

        public string Firstname { get; }

        public string MiddleName { get; }

        public string Surname { get; }

        public string PreferredFirstname { get; }

        public string PreferredSurname { get; }

        public string Ethinicity { get; }

        public string Nationality { get; }

        public string PlaceOfBirth { get; }

        public string DateOfBirth { get; }

        public string Gender { get; }

        public List<Identification> Identification { get; }

        public List<string> PersonTypes { get; }

        public bool IsPersonCautionaryAlert { get; }

        public bool IsTenureCautionaryAlert { get; }

        public List<Tenure> Tenures { get; }
    }
}
