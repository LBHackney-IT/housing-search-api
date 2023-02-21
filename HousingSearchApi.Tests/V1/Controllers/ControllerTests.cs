using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.Tests.V1.Controllers
{
    public abstract class ControllerTests
    {
        protected static T GetResultData<T>(IActionResult result)
        {
            return (T) (result as ObjectResult)?.Value;
        }

        protected static int? GetStatusCode(IActionResult result)
        {
            return (result as IStatusCodeActionResult).StatusCode;
        }
    }
}
