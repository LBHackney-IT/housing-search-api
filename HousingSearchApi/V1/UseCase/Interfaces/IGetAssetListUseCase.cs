using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;

namespace HousingSearchApi.V1.UseCase.Interfaces
{
    public interface IGetAssetListUseCase
    {
        Task<GetAssetListResponse> ExecuteAsync(GetAssetListRequest getPersonListRequest);
    }
}
