using System.Collections.Generic;
using Hackney.Shared.HousingSearch.Domain.Asset;

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
