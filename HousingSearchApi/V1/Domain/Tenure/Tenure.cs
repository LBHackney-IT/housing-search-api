using System.Collections.Generic;

namespace HousingSearchApi.V1.Domain.Tenure
{
    public class Tenure
    {
        public string Id { get; set; }
        public string PaymentReference { get; set; }
        public List<HouseholdMember> HouseholdMembers { get; set; }
        public TenuredAsset TenuredAsset { get; set; }
        public string StartOfTenureDate { get; set; }
        public string EndOfTenureDate { get; set; }
        public TenureType TenureType { get; set; }
    }
}
