using Hackney.Core.Logging;
using HousingSearchApi.V2.Gateways.Interfaces;
using System.Threading.Tasks;
using HousingSearchApi.V2.Domain.DTOs;


namespace HousingSearchApi.V2.UseCase;

public class SearchUseCase
{
    private readonly ISearchGateway _searchGateway;

    public SearchUseCase(ISearchGateway searchGateway)
    {
        _searchGateway = searchGateway;
    }

    [LogCall]
    public async Task<SearchResponseDto> ExecuteAsync(string indexName, SearchParametersDto searchParametersDto)
    {
        return await _searchGateway.Search(indexName, searchParametersDto).ConfigureAwait(false);
    }
}

