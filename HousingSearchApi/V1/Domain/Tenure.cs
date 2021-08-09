namespace HousingSearchApi.V1.Domain
{
    public class Tenure
    {
        public Tenure() { }

        public static Tenure Create(string id, string type, decimal totalBalance, string startDate, string endDate, string assetFullAddress, string postCode, string rentAccountNumber)
        {
            return new Tenure(id, type, totalBalance, startDate, endDate, assetFullAddress, postCode, rentAccountNumber);
        }

        private Tenure(string id, string type, decimal totalBalance, string startDate, string endDate, string assetFullAddress, string postCode, string rentAccountNumber)
        {
            Id = id;
            Type = type;
            TotalBalance = totalBalance;
            StartDate = startDate;
            EndDate = endDate;
            AssetFullAddress = assetFullAddress;
            PostCode = postCode;
            RentAccountNumber = rentAccountNumber;
        }

        public string Id { get; set; }

        public string Type { get; set; }

        public decimal TotalBalance { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string AssetFullAddress { get; set; }

        public string PostCode { get; set; }

        public string RentAccountNumber { get; set; }
    }
}
