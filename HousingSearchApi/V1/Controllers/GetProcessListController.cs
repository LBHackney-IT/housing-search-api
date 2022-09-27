using Amazon.Lambda.Core;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/search/processes")]
    [ApiController]
    public class GetProcessListController : BaseController
    {
        private readonly IGetProcessListUseCase _getProcessListUseCase;
        public GetProcessListController(IGetProcessListUseCase getProcessListUseCase)
        {
            _getProcessListUseCase = getProcessListUseCase;
        }

        [ProducesResponseType(typeof(APIResponse<GetProcessListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetProcessList([FromQuery] GetProcessListRequest request)
        {
            try
            {
                var processesearchResult = await _getProcessListUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIResponse<GetProcessListResponse>(processesearchResult);
                apiResponse.Total = processesearchResult.Total();

                return new OkObjectResult(apiResponse);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message + e.StackTrace);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
