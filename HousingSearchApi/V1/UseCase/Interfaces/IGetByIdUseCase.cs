using HousingSearchApi.V1.Boundary.Response;

namespace HousingSearchApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        ResponseObject Execute(int id);
    }
}
