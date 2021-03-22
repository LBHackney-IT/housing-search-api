using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SearchApi.V1.Domain
{
    public class GetPersonListResponse
    {
        public List<Person> Persons { get; set; }
    }

    public class GetPersonListRequest
    {
        [FromQuery(Name = "postcode")]
        public string PostCode { get; set; }

        [FromQuery(Name = "llpg-ref")]
        public string LLPGReference { get; set; }

        [FromQuery(Name = "prop-ref")]
        public string PropertyReference { get; set; }
    }
}
