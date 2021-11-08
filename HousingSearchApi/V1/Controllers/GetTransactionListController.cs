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

        [ProducesResponseType(typeof(APIResponse<GetTransactionListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetTransactionList([FromQuery] TransactionSearchRequest request)
        {
            try
            {
                var transactionSearchResult = await _getTransactionListUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIResponse<GetTransactionListResponse>(transactionSearchResult)
                {
                    Total = transactionSearchResult.Total()
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
