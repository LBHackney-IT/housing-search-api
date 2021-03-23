using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchApi.V1.Boundary.Responses;
using SearchApi.V1.Boundary.Responses.Metadata;
using SearchApi.V1.Domain;
using SearchApi.V1.UseCase.Interfaces;

namespace SearchApi.V1.Controllers
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
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        public async Task<IActionResult> GetPersonList([FromQuery] GetPersonListRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<Error>();
                foreach (var (key, value) in ModelState)
                {
                    var err = new Error();
                    foreach (var error in value.Errors)
                    {
                        err.FieldName = key;
                        err.Message = error.ErrorMessage;
                        errors.Add(err);
                    }
                }

                return new BadRequestObjectResult(new ErrorResponse(400, errors));
            }
            var rRequest = request;

            await Task.FromResult(0).ConfigureAwait(false);
            throw new NotImplementedException();
        }
    }
}
