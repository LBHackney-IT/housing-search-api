using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;

namespace HousingSearchApi.V1.UseCase.Interfaces
{
    public interface IGetAccountListUseCase
    {
        Task<GetAccountListResponse> ExecuteAsync(HousingSearchRequest getAccountListRequest);
    }
}
