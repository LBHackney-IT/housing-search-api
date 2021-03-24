using System.Threading.Tasks;
using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.Tests.V1.UseCase
{
    public interface IGetPersonListUseCase
    {
        Task<GetPersonListResponse> ExecuteAsync(GetPersonListRequest getPersonListRequest);
    }
}
