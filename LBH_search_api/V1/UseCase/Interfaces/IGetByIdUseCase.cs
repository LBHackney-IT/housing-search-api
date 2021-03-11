using SearchApi.V1.Boundary.Response;

namespace SearchApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        ResponseObject Execute(int id);
    }
}
