using System.Threading.Tasks;
using SearchApi.V1.Domain;

namespace SearchApi.Tests.V1.UseCase
{
    public interface IGetPersonListUseCase
    {
        Task<GetPersonListResponse> ExecuteAsync(GetPersonListRequest getPersonListRequest);
    }
}
