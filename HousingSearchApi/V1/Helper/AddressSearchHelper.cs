using HousingSearchApi.V1.Boundary.Requests;
using System;
using Hackney.Shared.HousingSearch.Domain.Asset;

namespace HousingSearchApi.V1.Helper
{
    public static class AddressSearchHelper
    {
        public static readonly Func<Asset, HousingSearchRequest, bool> MatchAddress = (asset, searchModel) =>
        {
            if (!string.IsNullOrEmpty(asset.AssetAddress.PostPreamble) && asset.AssetAddress.PostPreamble.ToLower().Contains(searchModel.SearchText.ToLower())) return true;

            if (!string.IsNullOrEmpty(asset.AssetAddress.AddressLine1) && asset.AssetAddress.AddressLine1.ToLower().Contains(searchModel.SearchText.ToLower())) return true;

            if (!string.IsNullOrEmpty(asset.AssetAddress.PostCode) && asset.AssetAddress.PostCode.ToLower().Replace(" ", "").Contains(searchModel.SearchText.ToLower().Replace(" ", ""))) return true;

            return false;
        };
    }

}
