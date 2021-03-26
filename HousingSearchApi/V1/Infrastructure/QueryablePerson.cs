using System.ComponentModel.DataAnnotations;
using Nest;

namespace HousingSearchApi.V1.Infrastructure
{
    public class QueryablePerson
    {
        [Text(Name = "firstname")]
        public string FirstName { get; set; }

        [Text(Name = "middleName")]
        public string MiddleName { get; set; }

        [Text(Name = "surname")]
        public string Surname { get; set; }

        [Text(Name = "dateOfBirth")]
        public string DateOfBirth { get; set; }
    }
}
