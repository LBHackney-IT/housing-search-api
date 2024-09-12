using System.Collections.Generic;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.UseCase.Interfaces
{
    public interface IGetUseCase
    {
        Task<IReadOnlyCollection<object>> ExecuteAsync(string indexName, string searchText);
    }
}
