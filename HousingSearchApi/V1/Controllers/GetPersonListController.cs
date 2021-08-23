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
using HousingSearchApi.V1.Helper;

namespace HousingSearchApi.V1.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/search/persons")]
    [ApiController]
    public class GetPersonListController : BaseController
    {
        private readonly IGetPersonListUseCase _getPersonListUseCase;

        public GetPersonListController(IGetPersonListUseCase getPersonListUseCase)
        {
            _getPersonListUseCase = getPersonListUseCase;
        }

        [ProducesResponseType(typeof(APIResponse<GetPersonListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetPersonList([FromQuery] HousingSearchRequest request)
        {
            try
            {
                var personsSearchResult = await _getPersonListUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIResponse<GetPersonListResponse>(personsSearchResult);
                apiResponse.Total = personsSearchResult.Total();

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
