using System.Collections.Generic;

namespace HousingSearchApi.V1.Domain.Asset
{
    public class Asset
    {
        public Asset() { }

        public static Asset Create(string assetId, AssetType assetType, string assetName,
            AssetAddress assetAddress, decimal totalDwellingRent,
            decimal totalNonDwellingRent, decimal totalServiceCharges, decimal totalRentalServiceCharges,
            decimal totalBalance, List<AssetProperty> properties)
        {
            return new Asset(assetId, assetType, assetName,
                assetAddress, totalDwellingRent,
                totalNonDwellingRent, totalServiceCharges, totalRentalServiceCharges,
                totalBalance, properties);
        }

        private Asset(string assetId, AssetType assetType, string assetName,
            AssetAddress assetAddress, decimal totalDwellingRent,
            decimal totalNonDwellingRent, decimal totalServiceCharges, decimal totalRentalServiceCharges,
            decimal totalBalance, List<AssetProperty> properties)
        {
            AssetId = assetId;
            AssetType = assetType;
            AssetName = assetName;
            AssetAddress = assetAddress;
            TotalDwellingRent = totalDwellingRent;
            TotalNonDwellingRent = totalNonDwellingRent;
            TotalServiceCharges = totalServiceCharges;
            TotalRentalServiceCharges = totalRentalServiceCharges;
            TotalBalance = totalBalance;
            Properties = properties;
        }

        public string AssetId { get; set; }

        public AssetType AssetType { get; set; }

        public string AssetName { get; set; }

        public List<AssetProperty> Properties { get; set; }

        public AssetAddress AssetAddress { get; set; }

        public decimal TotalDwellingRent { get; set; }

        public decimal TotalNonDwellingRent { get; set; }

        public decimal TotalServiceCharges { get; set; }

        public decimal TotalRentalServiceCharges { get; set; }

        public decimal TotalBalance { get; set; }
    }
}
