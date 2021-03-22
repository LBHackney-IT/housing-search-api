using Microsoft.AspNetCore.Mvc;

namespace SearchApi.V1.Domain
{
    public class GetPersonRequest
    {
        [FromQuery(Name = "postcode")]
        public string PostCode { get; set; }

        [FromQuery(Name = "llpg-ref")]
        public string LLPGReference { get; set; }

        [FromQuery(Name = "prop-ref")]
        public string PropertyReference { get; set; }
    }
}
