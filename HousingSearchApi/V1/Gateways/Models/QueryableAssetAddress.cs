using Nest;

namespace HousingSearchApi.V1.Gateways.Models
{
    public class QueryableAssetAddress
    {
        [Text(Name = "uprn")]
        public string Uprn { get; set; }

        [Text(Name = "addressLine1")]
        public string AddressLine1 { get; set; }

        [Text(Name = "addressLine2")]
        public string AddressLine2 { get; set; }

        [Text(Name = "addressLine3")]
        public string AddressLine3 { get; set; }

        [Text(Name = "addressLine4")]
        public string AddressLine4 { get; set; }

        [Text(Name = "postCode")]
        public string PostCode { get; set; }

        [Text(Name = "postPreamble")]
        public string PostPreamble { get; set; }
    }
}