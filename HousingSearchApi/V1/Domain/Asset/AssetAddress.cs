namespace HousingSearchApi.V1.Domain.Asset
{
    public class AssetAddress
    {
        public static AssetAddress Create(string uprn, string addressLine1, string addressLine2,
            string addressLine3, string addressLine4, string postCode, string postPreamble)
        {
            return new AssetAddress(uprn, addressLine1, addressLine2, addressLine3, addressLine4,
                postCode, postPreamble);
        }

        public AssetAddress()
        {

        }

        private AssetAddress(string uprn, string addressLine1, string addressLine2,
            string addressLine3, string addressLine4, string postCode, string postPreamble)
        {
            Uprn = uprn;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            AddressLine3 = addressLine3;
            AddressLine4 = addressLine4;
            PostCode = postCode;
            PostPreamble = postPreamble;
        }

        public string Uprn { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string PostCode { get; set; }
        public string PostPreamble { get; set; }
    }
}
