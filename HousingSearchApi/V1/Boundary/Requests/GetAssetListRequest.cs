using HousingSearchApi.V1.Domain;
using System.ComponentModel.DataAnnotations;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetAssetListRequest : APIRequest
    {
        /// <summary>
        /// AssetType for which we are looking [Block, Estate]
        /// </summary>
        /// <example>
        /// Block
        /// </example>
        [Required]
        public AssetType AssetType { get; set; }
    }
}
