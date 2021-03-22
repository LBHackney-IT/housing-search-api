using HousingSearchApi.V1.Boundary.Response;

namespace HousingSearchApi.V1.UseCase.Interfaces
{
    public interface IGetAllUseCase
    {
        ResponseObjectList Execute();
    }
}
