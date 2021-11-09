using System;
using HousingSearchApi.V1.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace HousingSearchApi.V1.Domain
{
    public class PrimaryTenants
    {
        /// <example>
        ///     793dd4ca-d7c4-4110-a8ff-c58eac4b90fa
        /// </example>
        [NonEmptyGuid]
        public Guid Id { get; set; }
        /// <example>
        ///     Smith Johnson
        /// </example>
        [Required]
        public string FullName { get; set; }
    }
}
