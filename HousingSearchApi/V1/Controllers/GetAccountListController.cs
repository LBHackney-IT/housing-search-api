using Amazon.Lambda.Core;
using Hackney.Core.Logging;
using Hackney.Shared.HousingSearch.Domain.Accounts;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace HousingSearchApi.V1.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/search/accounts")]
    [ApiController]
    public class GetAccountListController : BaseController
    {
        private readonly IGetAccountListUseCase _getAccountListUseCase;

        public GetAccountListController(IGetAccountListUseCase getAccountListUseCase)
        {
            _getAccountListUseCase = getAccountListUseCase;
        }

        [ProducesResponseType(typeof(APIResponse<Account>), 200)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [ProducesResponseType(typeof(APIResponse<InternalServerErrorException>), 500)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetAccountList([FromQuery] GetAccountListRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Input parameter is null!");

                var accountSearchResult = await _getAccountListUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIResponse<GetAccountListResponse>(accountSearchResult) { Total = accountSearchResult.Total() };

                return Ok(apiResponse);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message + e.StackTrace);
                return StatusCode((int) HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
