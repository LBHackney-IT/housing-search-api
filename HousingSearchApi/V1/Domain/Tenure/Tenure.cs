using System.Collections.Generic;
using System.Linq;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Helper;

namespace HousingSearchApi.V1.Domain.Tenure
{
    public class Tenure
    {
        public static Tenure Create(string id, string paymentReference, string startOfTenureDate, string endOfTenureDate,
            List<QueryableHouseholdMember> houseHoldMembers, QueryableTenuredAsset tenuredAsset, QueryableTenureType tenureType)
        {
            return new Tenure(id, paymentReference, startOfTenureDate, endOfTenureDate, houseHoldMembers,
                tenuredAsset, tenureType);
        }

        private Tenure(string id, string paymentReference, string startOfTenureDate, string endOfTenureDate,
            List<QueryableHouseholdMember> houseHoldMembers, QueryableTenuredAsset tenuredAsset, QueryableTenureType tenureType)
        {
            Id = id;
            PaymentReference = paymentReference;
            StartOfTenureDate = startOfTenureDate;
            EndOfTenureDate = endOfTenureDate;
            HouseholdMembers = houseHoldMembers.Select(HouseholdMember.Create).ToList();
            TenureType = TenureType.Create(tenureType);
            TenuredAsset = TenuredAsset.Create(tenuredAsset);
        }

        public string Id { get; set; }
        public string PaymentReference { get; set; }
        public List<HouseholdMember> HouseholdMembers { get; set; }
        public TenuredAsset TenuredAsset { get; set; }
        public string StartOfTenureDate { get; set; }
        public string EndOfTenureDate { get; set; }
        public TenureType TenureType { get; set; }
        public bool IsActive => TenureHelpers.IsTenureActive(EndOfTenureDate);
    }
}
