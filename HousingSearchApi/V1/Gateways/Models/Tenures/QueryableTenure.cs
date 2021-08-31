using System.Collections.Generic;
using Nest;

namespace HousingSearchApi.V1.Gateways.Models.Tenures
{
    public class QueryableTenure
    {
        public Domain.Tenure.Tenure Create()
        {
            return Domain.Tenure.Tenure.Create(Id, PaymentReference, StartOfTenureDate, EndOfTenureDate,
                HouseholdMembers, TenuredAsset, TenureType);
        }

        [Text(Name = "id")]
        public string Id { get; set; }
        public string PaymentReference { get; set; }
        public List<QueryableHouseholdMember> HouseholdMembers { get; set; }
        public QueryableTenuredAsset TenuredAsset { get; set; }
        public string StartOfTenureDate { get; set; }
        public string EndOfTenureDate { get; set; }
        public QueryableTenureType TenureType { get; set; }
    }
}
