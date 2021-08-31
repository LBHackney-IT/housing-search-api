using Nest;

namespace HousingSearchApi.V1.Gateways.Models.Assets
{
    public class QueryableTenuredAsset
    {
        [Text(Name = "fullAddress")]
        public string FullAddress { get; set; }
        public string Uprn { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
    }
}
