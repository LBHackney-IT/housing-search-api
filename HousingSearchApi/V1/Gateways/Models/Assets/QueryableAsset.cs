using System;
using Nest;
using Asset = Hackney.Shared.Asset.Asset;
using AssetAddress = Hackney.Shared.Asset.AssetAddress;
using Tenure = Hackney.Shared.Asset.AssetTenure;

namespace HousingSearchApi.V1.Gateways.Models.Assets
{
    public class QueryableAsset
    {
        public Asset Create()
        {
            var assetAddress = AssetAddress == null
                ? new AssetAddress()
                : new AssetAddress
                {
                    Uprn = AssetAddress.Uprn,
                    AddressLine1 = AssetAddress.AddressLine1,
                    AddressLine2 = AssetAddress.AddressLine2,
                    AddressLine3 = AssetAddress.AddressLine3,
                    AddressLine4 = AssetAddress.AddressLine4,
                    PostCode = AssetAddress.PostCode,
                    PostPreamble = AssetAddress.PostPreamble
                };

            var tenure = Tenure == null
                ? new Tenure()
                : new Tenure
                {
                    Id = Tenure.Id,
                    PaymentReference = Tenure.PaymentReference,
                    StartOfTenureDate = DateTime.Parse(Tenure.StartOfTenureDate),
                    EndOfTenureDate = DateTime.Parse(Tenure.EndOfTenureDate),
                    Type = Tenure.Type
                };

            return Asset.Create(
                Id,
                AssetId,
                AssetType,
                IsAssetCautionaryAlerted,
                assetAddress,
                tenure
            );
        }

        [Text(Name = "id")]
        public string Id { get; set; }

        [Text(Name = "assetId")]
        public string AssetId { get; set; }

        [Text(Name = "assetType")]
        public string AssetType { get; set; }

        [Text(Name = "isAssetCautionaryAlerted")]
        public bool IsAssetCautionaryAlerted { get; set; }

        public QueryableAssetAddress AssetAddress { get; set; }

        public QueryableTenure Tenure { get; set; }
    }
}
