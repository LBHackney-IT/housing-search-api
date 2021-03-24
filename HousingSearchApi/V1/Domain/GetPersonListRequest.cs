using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Domain
{
    public class GetPersonListResponse
    {
        public List<Person> Persons { get; set; }
    }

    public class GetPersonListRequest
    {
        [FromQuery(Name = "searchText")]
        [Required]
        public string SearchText { get; set; }
    }
}
