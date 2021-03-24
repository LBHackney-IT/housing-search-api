using System.Threading.Tasks;
using SearchApi.V1.Domain;

namespace SearchApi.V1.UseCase.Interfaces
{
    public interface IGetPersonListUseCase
    {
        Task<GetPersonListResponse> ExecuteAsync(GetPersonListRequest getPersonListRequest);
    }
}
