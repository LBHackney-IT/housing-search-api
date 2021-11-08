using System.Collections.Generic;
using Nest;

namespace HousingSearchApi.V1.Domain.QueryableModels
{
    public class QueryableTenure
    {
        [Text(Name = "fullAddress")]
        public string FullAddress { get; set; }
        [Text(Name = "primaryTenants")]
        public List<QueryablePrimaryTenant> PrimaryTenants { get; set; }
    }
}
