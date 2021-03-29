using Newtonsoft.Json;

namespace HousingSearchApi.V1.Domain
{
    public class Person
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("preferredFirstname")]
        public string PreferredFirstname { get; set; }

        [JsonProperty("preferredSurname")]
        public string PreferredSurname { get; set; }

        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; }
    }
}
