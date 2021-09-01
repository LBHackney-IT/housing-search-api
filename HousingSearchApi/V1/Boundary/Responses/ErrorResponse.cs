using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using Newtonsoft.Json;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class ErrorResponse
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("error")]
        public APIError Error { get; set; }

        public ErrorResponse() { }

        public ErrorResponse(ValidationResult validationResult)
        {
            var errors = validationResult.Errors
                .Select(validationResultError => new ValidationError(validationResultError)).ToList();

            Error = new APIError { IsValid = validationResult.IsValid, ValidationErrors = errors };
        }

        public ErrorResponse(IList<ValidationError> validationResult)
        {
            Error = new APIError { IsValid = !validationResult.Any(), ValidationErrors = validationResult };
        }
    }
}
