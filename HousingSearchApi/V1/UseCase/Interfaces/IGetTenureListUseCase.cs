using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;

namespace HousingSearchApi.V1.UseCase.Interfaces
{
    public interface IGetTenureListUseCase
    {
        Task<GetTenureListResponse> ExecuteAsync(HousingSearchRequest getPersonListRequest);
    }
}
