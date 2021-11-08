using System;
using System.ComponentModel.DataAnnotations;

namespace HousingSearchApi.V1.Boundary.Responses.Transactions
{
    public class Person
    {
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; }
    }
}
