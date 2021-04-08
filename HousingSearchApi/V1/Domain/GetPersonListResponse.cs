using System.Collections.Generic;

namespace HousingSearchApi.V1.Domain
{
    public class GetPersonListResponse
    {
        public List<Person> Persons { get; set; }

        public GetPersonListResponse()
        {
            Persons = new List<Person>();
        }
    }
}
