using System.Collections.Generic;
using Asset = Hackney.Shared.Asset.Asset;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetAssetListResponse
    {
        private long _total;

        public List<Asset> Assets { get; set; }

        public GetAssetListResponse()
        {
            Assets = new List<Asset>();
        }

        public void SetTotal(long total)
        {
            _total = total;
        }

        public long Total()
        {
            return _total;
        }
    }
}
