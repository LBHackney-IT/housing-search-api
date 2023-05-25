using Amazon.Lambda.Core;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/search/assetrelationships")]
    [ApiController]
    public class GetAssetRelationshipController : BaseController
    {
        private readonly IGetAssetRelationshipsUseCase _getAssetRelationshipsUseCase;

        public GetAssetRelationshipController(IGetAssetRelationshipsUseCase getAssetRelationshipsUseCase)
        {
            _getAssetRelationshipsUseCase = getAssetRelationshipsUseCase;
        }

        [ProducesResponseType(typeof(APIResponse<GetAssetRelationshipsResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NoContentResult>), 204)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(Microsoft.Extensions.Logging.LogLevel.Information)]
        public async Task<IActionResult> GetAssetRelationships([FromQuery] GetAssetRelationshipsRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return BadRequest("Request searchtext cannot be blank");

            try
            {
                var assetsSearchResult = await _getAssetRelationshipsUseCase.ExecuteAsync(request).ConfigureAwait(false);

                if (!assetsSearchResult.ChildAssets.Any()) return new NoContentResult();

                return new OkObjectResult(assetsSearchResult);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message + e.StackTrace);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
