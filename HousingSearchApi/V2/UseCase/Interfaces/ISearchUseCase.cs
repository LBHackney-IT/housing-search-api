using System.Collections.Generic;
using System.Threading.Tasks;
using HousingSearchApi.V2.Domain.DTOs;

namespace HousingSearchApi.V2.UseCase.Interfaces;

public interface ISearchUseCase
{
    Task<IReadOnlyCollection<object>> ExecuteAsync(string indexName, SearchParametersDto searchParametersDto);
}

