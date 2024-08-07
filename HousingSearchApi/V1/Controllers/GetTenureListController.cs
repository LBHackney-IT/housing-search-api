using Amazon.Lambda.Core;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IGetTenureListSetsUseCase _getTenureListSetsUseCase;

        public GetTenureListController(IGetTenureListUseCase getTenureListUseCase, IGetTenureListSetsUseCase getTenureListSetsUseCase)
        {
            _getTenureListUseCase = getTenureListUseCase;
            _getTenureListSetsUseCase = getTenureListSetsUseCase;
        }

        [ProducesResponseType(typeof(APIResponse<GetTenureListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(Microsoft.Extensions.Logging.LogLevel.Information)]
        public async Task<IActionResult> GetTenureList([FromQuery] GetTenureListRequest request)
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

        /// <summary>
        /// Last hit id paging is only supported when sorting by tenureStartDate. Any other type of paging is not supported. Page size can be set however when using tenureStartDate sorting with last hit id paging
        /// </summary>
        [ProducesResponseType(typeof(APIAllTenureResponse<GetAllTenureListResponse>), 200)]
        [ProducesResponseType(typeof(APIAllTenureResponse<BadRequestObjectResult>), 400)]
        [Route("all")]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(Microsoft.Extensions.Logging.LogLevel.Information)]

        public async Task<IActionResult> GetAllTenureList([FromQuery] GetAllTenureListRequest request)
        {
            try
            {
                var searchResults = await _getTenureListSetsUseCase.ExecuteAsync(request).ConfigureAwait(false);

                var response = new APIAllTenureResponse<GetAllTenureListResponse>()
                {
                    Total = searchResults.Total(),
                    LastHitId = searchResults.LastHitId,
                    LastHitTenureStartDate = searchResults.LastHitTenureStartDate,
                    Results = searchResults
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
