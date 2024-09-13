using System.Collections.Generic;
using System.Threading.Tasks;

namespace HousingSearchApi.V2.UseCase.Interfaces;

public interface ISearchUseCase
{
    Task<IReadOnlyCollection<object>> ExecuteAsync(string indexName, string searchText);
}

