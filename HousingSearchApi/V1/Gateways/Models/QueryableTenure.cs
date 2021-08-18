using System.Collections.Generic;
using System.Linq;
using HousingSearchApi.V1.Domain.Tenure;
using Nest;
using Tenure = HousingSearchApi.V1.Domain.Person.PersonTenure;

namespace HousingSearchApi.V1.Gateways.Models
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
