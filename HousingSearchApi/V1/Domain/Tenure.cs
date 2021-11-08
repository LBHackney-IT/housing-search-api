using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Hackney.Shared.Tenure.Domain;

namespace HousingSearchApi.V1.Domain
{
    public class Tenure
    {
        /// <example>
        ///     31245
        /// </example>
        [NotNull]
        public string TenureId { get; set; }

        /// <example>
        ///     Introductory
        /// </example>
        [NotNull]
        public TenureType TenureType { get; set; }

        /// <example>
        ///     285 Avenue, 315 Amsterdam
        /// </example>
        [NotNull]
        public string FullAddress { get; set; }

        public IEnumerable<PrimaryTenant> PrimaryTenants { get; set; }
    }


}
