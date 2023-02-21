using Hackney.Shared.HousingSearch.Domain.Asset;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousingSearchApi.V1.Helper
{
    public class CustomAddressSorter : ICustomAddressSorter
    {
        private readonly IComparer<AssetAddress> _comparer;

        public CustomAddressSorter(IComparer<AssetAddress> comparer)
        {
            _comparer = comparer;
        }

        public void FilterResponse(HousingSearchRequest searchModel, GetAllAssetListResponse content)
        {
            if (content == null || content.Assets == null) return;

            content.Assets = content.Assets
                .Where(x => _matchAddress(x, searchModel))
                .OrderBy(x => x.AssetType)
                .ThenBy(x => x.AssetAddress.PostCode)
                .ThenBy(y => y.AssetAddress, _comparer)
                .ToList();
        }

        private static readonly Func<Asset, HousingSearchRequest, bool> _matchAddress = (asset, searchModel) =>
        {
            if (!string.IsNullOrEmpty(asset.AssetAddress.PostPreamble) && asset.AssetAddress.PostPreamble.ToLower().Contains(searchModel.SearchText.ToLower())) return true;

            if (!string.IsNullOrEmpty(asset.AssetAddress.AddressLine1) && asset.AssetAddress.AddressLine1.ToLower().Contains(searchModel.SearchText.ToLower())) return true;

            if (!string.IsNullOrEmpty(asset.AssetAddress.PostCode) && asset.AssetAddress.PostCode.ToLower().Replace(" ", "").Contains(searchModel.SearchText.ToLower().Replace(" ", ""))) return true;

            return false;
        };
    }
}
