namespace HousingSearchApi.V1.Domain.Person
{
    public class Tenure
    {
        public Tenure() { }

        public static Tenure Create(string id, string type, string startDate, string endDate, string assetFullAddress)
        {
            return new Tenure(id, type, startDate, endDate, assetFullAddress);
        }

        private Tenure(string id, string type, string startDate, string endDate, string assetFullAddress)
        {
            Id = id;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            AssetFullAddress = assetFullAddress;
        }

        public string Id { get; set; }

        public string Type { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string AssetFullAddress { get; set; }

    }
}
