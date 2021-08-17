using System.Collections.Generic;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Helper
{
    public static class ModelValidationHelper
    {
        public static IActionResult Return400ForInvalidRequest()
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
    }
}
