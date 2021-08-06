using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Gateways.Interfaces
{
    public interface ISearchAssetsGateway
    {
        Task<GetAssetListResponse> GetListOfAssets(GetAssetListRequest query);
    }
}
