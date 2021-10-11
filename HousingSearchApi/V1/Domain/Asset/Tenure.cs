using HousingSearchApi.V1.Helper;

namespace HousingSearchApi.V1.Domain.Asset
{
    public class Tenure
    {
        public static Tenure Create(string id, string paymentReference, string startOfTenureDate,
            string endOfTenureDate, string tenureType, string propertyReference)
        {
            return new Tenure(
                id,
                paymentReference,
                startOfTenureDate,
                endOfTenureDate,
                tenureType,
                propertyReference
            );
        }

        public Tenure() { }

        private Tenure(string id, string paymentReference, string startOfTenureDate, string endOfTenureDate,
            string tenureType, string propertyReference)
        {
            Id = id;
            PaymentReference = paymentReference;
            StartOfTenureDate = startOfTenureDate;
            EndOfTenureDate = endOfTenureDate;
            Type = tenureType;
            PropertyReference = propertyReference;
        }

        public string Id { get; set; }
        public string PaymentReference { get; set; }
        public string StartOfTenureDate { get; set; }
        public string EndOfTenureDate { get; set; }
        public string Type { get; set; }
        public bool IsActive => TenureHelpers.IsTenureActive(EndOfTenureDate);
        public string PropertyReference { get; set; }
    }
}
