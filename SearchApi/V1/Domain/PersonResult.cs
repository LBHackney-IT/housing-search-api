using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SearchApi.V1.Domain
{
    public class Person
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

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

        [JsonProperty("identifications")]
        public List<Identification> Identifications { get; set; }

        [JsonProperty("personTypes")]
        public List<string> PersonTypes { get; set; }

        [JsonProperty("isPersonCautionaryAlert")]
        public bool IsPersonCautionaryAlert { get; set; }

        [JsonProperty("isTenureCautionaryAlert")]
        public bool IsTenureCautionaryAlert { get; set; }

        [JsonProperty("tenures")]
        public List<Tenure> Tenures { get; set; }
    }


}
