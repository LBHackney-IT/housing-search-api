using System.Collections.Generic;
using System.Threading.Tasks;
using HousingSearchApi.V2.Domain.DTOs;

namespace HousingSearchApi.V2.Gateways.Interfaces;

public interface ISearchGateway
{
    Task<SearchResponseDto> Search(string indexName, SearchParametersDto searchParametersDto);
}
