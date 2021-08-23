using Amazon.Lambda.Core;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
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
    [Route("api/v1/search/tenures")]
    [ApiController]
    public class GetTenureListController : BaseController
    {
        private readonly IGetTenureListUseCase _getTenureListUseCase;

        public GetTenureListController(IGetTenureListUseCase getTenureListUseCase)
        {
            _getTenureListUseCase = getTenureListUseCase;
        }

        [ProducesResponseType(typeof(APIResponse<GetTenureListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetTenureList([FromQuery] HousingSearchRequest request)
        {
            try
            {
                var tenuresSearchResult = await _getTenureListUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIResponse<GetTenureListResponse>(tenuresSearchResult);
                apiResponse.Total = tenuresSearchResult.Total();

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
