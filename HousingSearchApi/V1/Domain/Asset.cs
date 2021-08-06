using System.Collections.Generic;

namespace HousingSearchApi.V1.Domain
{
    public class Asset
    {
        public Asset()
        {
        }

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

        public string AssetId { get; }

        public AssetType AssetType { get; }

        public string AssetName { get; }

        public List<AssetProperty> Properties { get; }

        public AssetAddress AssetAddress { get; }

        public decimal TotalDwellingRent { get; }

        public decimal TotalNonDwellingRent { get; }

        public decimal TotalServiceCharges { get; }

        public decimal TotalRentalServiceCharges { get; }

        public decimal TotalBalance { get; }
    }
}
