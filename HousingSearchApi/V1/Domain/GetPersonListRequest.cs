using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

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

    public class GetPersonListRequest
    {
        [FromQuery(Name = "searchText")]
        [MinLength(2)]
        public string SearchText { get; set; }
    }
}