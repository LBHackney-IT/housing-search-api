using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/search/persons")]
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
        public async Task<IActionResult> GetPersonList([FromQuery] GetPersonListRequest request)
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
                var personsSearchResult = await _getPersonListUseCase.ExecuteAsync(request).ConfigureAwait(false); ;
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
