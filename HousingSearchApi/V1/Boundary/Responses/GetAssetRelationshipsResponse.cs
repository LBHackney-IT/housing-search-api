using System.Collections.Generic;
using Hackney.Shared.HousingSearch.Domain.Asset;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetAssetRelationshipsResponse
    {
        public List<Asset> ChildAssets { get; set; }

        public GetAssetRelationshipsResponse()
        {
            ChildAssets = new List<Asset>();
        }
    }
}
