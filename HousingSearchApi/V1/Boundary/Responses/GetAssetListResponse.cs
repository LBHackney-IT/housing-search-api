using HousingSearchApi.V1.Domain.Asset;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetAssetListResponse
    {
        public long Total { get; set; }

        public List<Asset> Assets { get; set; }

        public GetAssetListResponse()
        {
            Assets = new List<Asset>();
        }
    }
}
