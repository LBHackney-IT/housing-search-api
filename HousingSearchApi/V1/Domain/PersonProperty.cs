namespace HousingSearchApi.V1.Domain
{
    public class PersonProperty
    {
        public PersonProperty() { }

        public static PersonProperty Create(string id, string type, decimal totalBalance, string assetFullAddress, string postCode, string rentAccountNumber)
        {
            return new PersonProperty(id, type, totalBalance, assetFullAddress, postCode, rentAccountNumber);
        }

        private PersonProperty(string id, string type, decimal totalBalance, string assetFullAddress, string postCode, string rentAccountNumber)
        {
            Id = id;
            Type = type;
            TotalBalance = totalBalance;
            AssetFullAddress = assetFullAddress;
            PostCode = postCode;
            RentAccountNumber = rentAccountNumber;
        }

        public string Id { get; set; }

        public string Type { get; set; }

        public decimal TotalBalance { get; set; }

        public string AssetFullAddress { get; set; }

        public string PostCode { get; set; }

        public string RentAccountNumber { get; set; }
    }
}
