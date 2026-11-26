using Hackney.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using HousingSearchApi.V2.Domain.DTOs;
using HousingSearchApi.V2.UseCase;
using System;
using System.Linq;

namespace HousingSearchApi.V2.Controllers;

[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v2/search/")]
[ApiController]
public class SearchController : Controller
{
    // List of query parameter names that are considered standard search parameters to not be included in filters
    private static readonly HashSet<string> _searchParameterNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "searchText",
        "page",
        "pageSize",
    };

    private readonly SearchUseCase _searchUseCase;

    public SearchController(SearchUseCase searchUseCase)
    {
        _searchUseCase = searchUseCase;
    }

    [LogCall(Microsoft.Extensions.Logging.LogLevel.Information)]
    [HttpGet("{indexName}")]
    public async Task<IActionResult> Search(string indexName, [FromQuery] SearchParametersDto searchParametersDto)
    {
        searchParametersDto.Filters = Request.Query
            .Where(parameter => !_searchParameterNames.Contains(parameter.Key))
            .GroupBy(parameter => parameter.Key, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => group.SelectMany(parameter => parameter.Value).ToArray(),
                StringComparer.OrdinalIgnoreCase
            );

        var searchResults = await _searchUseCase.ExecuteAsync(indexName, searchParametersDto).ConfigureAwait(false);

        var response = new
        {
            Results = new Dictionary<string, IReadOnlyCollection<object>>
            {
                [indexName] = searchResults.Documents
            },
            searchResults.Total
        };

        return new OkObjectResult(response);
    }
}

