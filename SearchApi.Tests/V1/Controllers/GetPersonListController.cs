using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchApi.Tests.V1.Boundary.Responses.Metadata;
using SearchApi.Tests.V1.UseCase;
using SearchApi.V1.Controllers;
using SearchApi.V1.Domain;

namespace SearchApi.Tests.V1.Controllers
{
    [ApiVersion("2")]
    [Produces("application/json")]
    [Route("api/v1/assets")]
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
            var rRequest = request;

            await Task.FromResult(0).ConfigureAwait(false);
            throw new NotImplementedException();
        }
    }
}
