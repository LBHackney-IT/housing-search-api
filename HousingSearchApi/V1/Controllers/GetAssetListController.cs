using Amazon.Lambda.Core;
// TODO: 1 Return when last commit
//using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            if (!ModelState.IsValid)
            {
                var errors = new List<ValidationError>();

                var err = new ValidationError();

                err.FieldName = "Insufficient characters";
                errors.Add(err);

                return new BadRequestObjectResult(new ErrorResponse(errors)
                {
                    StatusCode = 400
                });
            }

            try
            {
                var assetsSearchResult = await _getAssetListUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIResponse<GetAssetListResponse>(assetsSearchResult);
                apiResponse.Total = assetsSearchResult.Total();

                // TODO: Maybe move to middleware?
                Response.Headers.Add("x-page-number", request.PageNumber.ToString());
                Response.Headers.Add("x-page-size", request.PageSize.ToString());

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
