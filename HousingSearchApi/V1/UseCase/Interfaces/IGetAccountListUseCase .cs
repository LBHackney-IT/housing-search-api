using System.Collections.Generic;
using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.UseCase.Interfaces
{
    public interface IGetAccountListUseCase
    {
        Task<APIResponse<List<Account>>> ExecuteAsync(GetAccountListRequest getAccountListRequest);
    }
}
