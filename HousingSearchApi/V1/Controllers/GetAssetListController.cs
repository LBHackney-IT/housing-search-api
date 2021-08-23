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
    [Route("api/v1/search/assets")]
    [ApiController]
    public class GetAssetListController : BaseController
    {
        private readonly IGetAssetListUseCase _getAssetListUseCase;

        public GetAssetListController(IGetAssetListUseCase getAssetListUseCase)
        {
            _getAssetListUseCase = getAssetListUseCase;
        }

        [ProducesResponseType(typeof(APIResponse<GetAssetListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetAssetList([FromQuery] HousingSearchRequest request)
        {
            try
            {
                var assetsSearchResult = await _getAssetListUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIResponse<GetAssetListResponse>(assetsSearchResult);
                apiResponse.Total = assetsSearchResult.Total();

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
