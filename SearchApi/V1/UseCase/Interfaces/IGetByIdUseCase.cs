using SearchApi.V1.Boundary.Responses;

namespace SearchApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        ResponseObject Execute(int id);
    }
}
