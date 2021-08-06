using Nest;

namespace HousingSearchApi.V1.Gateways.Models
{
    public class QueryableAssetProperty
    {
        [Text(Name = "name")]
        public string Name { get; set; }

        [Text(Name = "totalBalance")]
        public decimal TotalBalance { get; set; }

        [Text(Name = "assetAddress")]
        public QueryableAssetAddress AssetAddress { get; set; }
    }
}
