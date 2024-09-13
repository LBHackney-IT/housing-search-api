using Amazon.Lambda.Core;
using Hackney.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using HousingSearchApi.V2.Domain.DTOs;
using HousingSearchApi.V2.UseCase.Interfaces;

namespace HousingSearchApi.V2.Controllers;

[ApiVersion("1")]
[Produces("application/json")]
[Route("api/v2/search/")]
[ApiController]
public class SearchController : Controller
{
    private readonly ISearchUseCase _searchUseCase;

    public SearchController(ISearchUseCase searchUseCase)
    {
        _searchUseCase = searchUseCase;
    }

    [LogCall(Microsoft.Extensions.Logging.LogLevel.Information)]
    [HttpGet("{indexName}")]
    public async Task<IActionResult> Search(string indexName, [FromQuery] SearchParametersDto searchParametersDto)
    {
        try
        {
            var searchResults = await _searchUseCase.ExecuteAsync(indexName, searchParametersDto).ConfigureAwait(false);

            var response = new
            {
                Results = searchResults,
                Total = searchResults.Count
            };

            return new OkObjectResult(response);
        }
        catch (Exception e)
        {
            LambdaLogger.Log(e.Message + e.StackTrace);
            return new BadRequestObjectResult(e.Message);
        }
    }
}

