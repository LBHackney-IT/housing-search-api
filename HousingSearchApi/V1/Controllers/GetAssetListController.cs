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
        private readonly IGetAssetListSetsUseCase _getAssetListSetsUseCase;

        public GetAssetListController(IGetAssetListUseCase getAssetListUseCase, IGetAssetListSetsUseCase getAssetListSetsUseCase)
        {
            _getAssetListUseCase = getAssetListUseCase;
            _getAssetListSetsUseCase = getAssetListSetsUseCase;
        }

        [ProducesResponseType(typeof(APIResponse<GetAssetListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(Microsoft.Extensions.Logging.LogLevel.Information)]
        public async Task<IActionResult> GetAssetList([FromQuery] GetAssetListRequest request)
        {
            // Validate query parameters

            // conditionally override pageSize to '400'
            // ignore page, sortBy, isDesc

            if (request.UseCustomSorting && RequestIncludesOtherSortParameters(request))
            {
                return BadRequest($"UseCustomSorting is True. You cannot use other sort parameters, such as {nameof(request.Page)}, or {nameof(request.PageSize)}");
            }

            try
            {
                var assetsSearchResult = await _getAssetListUseCase
                    .ExecuteAsync(request)
                    .ConfigureAwait(false);

                var apiResponse = new APIResponse<GetAssetListResponse>(assetsSearchResult)
                {
                    Total = assetsSearchResult.Total()
                };

                return new OkObjectResult(apiResponse);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message + e.StackTrace);
                return new BadRequestObjectResult(e.Message);
            }
        }

        private static bool RequestIncludesOtherSortParameters(GetAssetListRequest request)
        {
            return request.Page != 0
                || !string.IsNullOrEmpty(request.SortBy);
        }

        [ProducesResponseType(typeof(APIResponse<GetAllAssetListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [Route("all")]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(Microsoft.Extensions.Logging.LogLevel.Information)]
        public async Task<IActionResult> GetAllAssetList([FromQuery] GetAllAssetListRequest request)
        {
            try
            {
                var assetsSearchResult = await _getAssetListSetsUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIAllResponse<GetAllAssetListResponse>(assetsSearchResult)
                {
                    Total = assetsSearchResult.Total(),
                    LastHitId = assetsSearchResult.LastHitId()
                };

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
