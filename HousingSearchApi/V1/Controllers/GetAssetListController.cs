using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/search/assets")]
    public class GetAssetListController : BaseController
    {
        private readonly IGetAssetListUseCase _getAssetListUseCase;

        public GetAssetListController(IGetAssetListUseCase getAssetListUseCase)
        {
            _getAssetListUseCase = getAssetListUseCase;
        }

        /// <summary>
        /// Get list of assets by provided filters
        /// </summary>
        /// <param name="request">Filter model</param>
        /// <response code="200">OK. Asset list weas received successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(GetAssetListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetAssetList([FromQuery] GetAssetListRequest request)
        {
            return await UseErrorHandling(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseErrorResponse(GetErrorMessage(ModelState), HttpStatusCode.BadRequest));
                }

                var assetsSearchResult = await _getAssetListUseCase.ExecuteAsync(request).ConfigureAwait(false);

                // TODO: Maybe move to middleware?
                Response.Headers.Add("x-page-number", request.PageNumber.ToString());
                Response.Headers.Add("x-page-size", request.PageSize.ToString());

                return Ok(assetsSearchResult);

            }).ConfigureAwait(false);
        }
    }
}
