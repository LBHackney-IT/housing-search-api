using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Controllers
{
    [ApiVersion("2")]
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
                foreach (var (key, value) in ModelState)
                {
                    var err = new ValidationError();
                    foreach (var error in value.Errors)
                    {
                        err.FieldName = key;
                        err.Message = error.ErrorMessage;
                        errors.Add(err);
                    }
                }

                return new BadRequestObjectResult(new ErrorResponse(errors));
            }

            try
            {
                var response = await _getPersonListUseCase.ExecuteAsync(request);
                return new OkObjectResult(new APIResponse<GetPersonListResponse>(response));
            }
            catch (BadRequestException e)
            {
                return new BadRequestObjectResult(new ErrorResponse(e.ValidationResponse));
            }
        }
    }
}
