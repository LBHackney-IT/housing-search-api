using HousingSearchApi.V1.Gateways.Models.Tenures;

namespace HousingSearchApi.V1.Domain.Tenure
{
    public class TenureType
    {
        public string Description { get; set; }
        public string Code { get; set; }

        public static TenureType Create(QueryableTenureType tenureType)
        {
            return new TenureType(tenureType);
        }

        public TenureType()
        {

        }

        private TenureType(QueryableTenureType tenureType)
        {
            Description = tenureType?.Description;
            Code = tenureType?.Code;
        }
    }
}
