using LBH_search_api.V1.Boundary.Response;

namespace LBH_search_api.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        ResponseObject Execute(int id);
    }
}
