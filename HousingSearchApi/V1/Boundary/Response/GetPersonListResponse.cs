using System.Collections.Generic;
using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.Boundary.Response
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
