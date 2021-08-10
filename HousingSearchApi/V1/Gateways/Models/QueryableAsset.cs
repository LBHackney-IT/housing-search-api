using HousingSearchApi.V1.Domain;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace HousingSearchApi.V1.Gateways.Models
{
    public class QueryableAsset
    {
        public Asset Create()
        {
            var properties = Properties == null ? new List<AssetProperty>() :
                Properties.Select(x => AssetProperty.Create(x?.Name, x.TotalBalance,
                    AssetAddress.Create(x.AssetAddress.Uprn, x.AssetAddress.AddressLine1, x.AssetAddress.AddressLine2,
                                        x.AssetAddress.AddressLine3, x.AssetAddress.AddressLine4,
                                        x.AssetAddress.PostCode, x.AssetAddress.PostPreamble))).ToList();

            var assetAddress = AssetAddress.Create(QueryableAssetAddress.Uprn, QueryableAssetAddress.AddressLine1,
                                                   QueryableAssetAddress.AddressLine2, QueryableAssetAddress.AddressLine3,
                                                   QueryableAssetAddress.AddressLine4, QueryableAssetAddress.PostCode,
                                                   QueryableAssetAddress.PostPreamble);

            return Asset.Create(AssetId, AssetType, AssetName, assetAddress, TotalDwellingRent, TotalNonDwellingRent,
                TotalServiceCharges, TotalRentalServiceCharges, TotalBalance, properties);
        }

        [Text(Name = "assetId")]
        public string AssetId { get; set; }

        [Text(Name = "assetType")]
        public AssetType AssetType { get; set; }

        [Text(Name = "assetName")]
        public string AssetName { get; set; }

        [Text(Name = "assetAddress")]
        public QueryableAssetAddress QueryableAssetAddress { get; set; }

        [Text(Name = "totalDwellingRent")]
        public decimal TotalDwellingRent { get; set; }

        [Text(Name = "totalNonDwellingRent")]
        public decimal TotalNonDwellingRent { get; set; }

        [Text(Name = "totalServiceCharges")]
        public decimal TotalServiceCharges { get; set; }

        [Text(Name = "totalRentalServiceCharges")]
        public decimal TotalRentalServiceCharges { get; set; }

        [Text(Name = "totalBalance")]
        public decimal TotalBalance { get; set; }

        [Text(Name = "properties")]
        public List<QueryableAssetProperty> Properties { get; set; }
    }
}
