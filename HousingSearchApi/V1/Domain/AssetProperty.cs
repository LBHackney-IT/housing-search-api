namespace HousingSearchApi.V1.Domain
{
    public class AssetProperty
    {
        public AssetProperty()
        {
        }

        public static AssetProperty Create(string name, decimal totalBalance, AssetAddress assetAddress)
        {
            return new AssetProperty(name, totalBalance, assetAddress);
        }

        private AssetProperty(string name, decimal totalBalance, AssetAddress assetAddress)
        {
            Name = name;
            TotalBalance = totalBalance;
            AssetAddress = assetAddress;
        }

        public string Name { get; set; }

        public decimal TotalBalance { get; set; }

        public AssetAddress AssetAddress { get; set; }
    }
}
