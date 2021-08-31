namespace HousingSearchApi.V1.Domain.Asset
{
    public class Asset
    {
        public static Asset Create(string id, string assetId, string assetType,
            bool isAssetCautionaryAlerted, AssetAddress assetAddress, Tenure tenure)
        {
            return new Asset(id, assetId, assetType, isAssetCautionaryAlerted, assetAddress, tenure);
        }

        public Asset()
        {

        }

        private Asset(string id, string assetId, string assetType,
            bool isAssetCautionaryAlerted, AssetAddress assetAddress, Tenure tenure)
        {
            Id = id;
            AssetId = assetId;
            AssetType = assetType;
            IsAssetCautionaryAlerted = isAssetCautionaryAlerted;
            AssetAddress = assetAddress;
            Tenure = tenure;
        }

        public string Id { get; set; }

        public string AssetId { get; set; }

        public string AssetType { get; set; }

        public bool IsAssetCautionaryAlerted { get; set; }

        public AssetAddress AssetAddress { get; set; }

        public Tenure Tenure { get; set; }
    }
}
