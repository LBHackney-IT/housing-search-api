using System.Collections.Generic;
using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.Boundary.Response
{
    public class GetPersonListResponse
    {
        private long _total;

        public List<Person> Persons { get; set; }

        public GetPersonListResponse()
        {
            Persons = new List<Person>();
        }

        public void SetTotal(long total)
        {
            _total = total;
        }

        public long Total()
        {
            return _total;
        }
    }
}
