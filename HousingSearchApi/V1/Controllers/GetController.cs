using Amazon.Lambda.Core;
using Hackney.Core.Logging;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static System.Int16;
using HttpUtility = System.Web.HttpUtility;

namespace HousingSearchApi.V1.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/search/free")]
    [ApiController]
    public class GetController : BaseController
    {
        private readonly IGetUseCase _getUseCase;

        public GetController(IGetUseCase getUseCase)
        {
            _getUseCase = getUseCase;
        }

        [LogCall(Microsoft.Extensions.Logging.LogLevel.Information)]
        [HttpGet("{indexName}")]
        public async Task<IActionResult> GetList(string indexName)
        {
            try
            {
                var query = Request.QueryString.Value ?? string.Empty;
                var searchText = HttpUtility.ParseQueryString(query).Get("searchText") ?? string.Empty;
                var pageSize = Parse(HttpUtility.ParseQueryString(query).Get("pageSize") ?? "40");

                var searchResults = await _getUseCase.ExecuteAsync(indexName, searchText).ConfigureAwait(false);

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
}
