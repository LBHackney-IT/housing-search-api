using Amazon.Lambda.Core;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Boundary.Responses.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using HousingSearchApi.V1.UseCase.Interfaces;

namespace HousingSearchApi.V1.Controllers
{
    /// <summary>
    /// Controller for performing flexible transaction search
    /// </summary>
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/search/transactions")]
    [ApiController]
    public class GetTransactionListController : BaseController
    {
        private readonly IGetTransactionListUseCase _getTransactionListUseCase;

        public GetTransactionListController(IGetTransactionListUseCase getTransactionListUseCase)
        {
            _getTransactionListUseCase = getTransactionListUseCase;
        }

        /// <summary>
        /// Paginated search for transactions based on searchText
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Transactions list</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(APIResponse<GetTransactionListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetTransactionList([FromQuery] GetTransactionSearchRequest request)
        {
            try
            {
                var transactionSearchResult = await _getTransactionListUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIResponse<GetTransactionListResponse>(transactionSearchResult)
                {
                    Total = transactionSearchResult.Total()
                };

                return Ok(apiResponse);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message + e.StackTrace);

                return BadRequest(e.Message);
            }
        }
    }
}
