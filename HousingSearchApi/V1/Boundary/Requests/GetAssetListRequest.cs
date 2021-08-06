using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAssetListRequest : APIRequest
    {
        public AssetType AssetType { get; set; }
    }
}
