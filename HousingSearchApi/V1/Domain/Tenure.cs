namespace HousingSearchApi.V1.Domain
{
    public class Tenure
    {
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

        public string Id { get; }

        public string Type { get; }

        public string StartDate { get; }

        public string EndDate { get; }

        public string AssetFullAddress { get; }

    }
}
