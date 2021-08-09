// TODO: 1 Return when last commit
//using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        [ProducesResponseType(typeof(APIResponse<GetAssetListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        // TODO: 1 Return when last commit
        //[LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetAssetList([FromQuery] GetAssetListRequest request)
        {
            return await UseErrorHandling(async() =>  
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
