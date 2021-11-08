using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.UseCase.Interfaces
{
    public interface IGetAccountListUseCase
    {
        Task<APIResponse<Account>> ExecuteAsync(GetAccountListRequest getAccountListRequest);
    }
}
