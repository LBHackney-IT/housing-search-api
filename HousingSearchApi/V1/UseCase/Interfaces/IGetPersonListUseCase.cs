using System.Threading.Tasks;
using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.UseCase.Interfaces
{
    public interface IGetPersonListUseCase
    {
        Task<GetPersonListResponse> ExecuteAsync(GetPersonListRequest getPersonListRequest);
    }
}
