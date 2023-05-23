using System.Collections.Generic;
using Hackney.Shared.HousingSearch.Domain.Asset;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetAssetRelationshipsResponse
    {
        private long _total;

        public List<Asset> ParentAssets { get; set; }
        public List<Asset> ChildAssets { get; set; }
        public List<Asset> RootAsset { get; set; }

        public GetAssetRelationshipsResponse()
        {
            ParentAssets = new List<Asset>();
            ChildAssets = new List<Asset>();
            RootAsset = new List<Asset>();
        }

        public void SetTotal(long total)
        {
            _total = total < 0 ? 0 : total;
        }

        public long Total()
        {
            return _total;
        }
    }
}
