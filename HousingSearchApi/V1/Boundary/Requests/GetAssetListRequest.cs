using HousingSearchApi.V1.Domain;
using System.ComponentModel.DataAnnotations;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAssetListRequest : APIRequest
    {
        [Required]
        public AssetType AssetType { get; set; }
    }
}
