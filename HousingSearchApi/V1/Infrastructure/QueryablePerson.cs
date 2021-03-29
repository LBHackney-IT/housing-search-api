using Nest;

namespace HousingSearchApi.V1.Infrastructure
{
    public class QueryablePerson
    {
        [Text(Name = "_id")]
        public string Id
        { get; set; }

        [Text(Name = "firstname")]
        public string FirstName { get; set; }

        [Text(Name = "middleName")]
        public string MiddleName { get; set; }

        [Text(Name = "surname")]
        public string Surname { get; set; }

        [Text(Name = "preferredFirstname")]
        public string PreferredFirstName { get; set; }

        [Text(Name = "preferredSurname")]
        public string PreferredSurname { get; set; }

        [Text(Name = "dateOfBirth")]
        public string DateOfBirth { get; set; }
    }
}
