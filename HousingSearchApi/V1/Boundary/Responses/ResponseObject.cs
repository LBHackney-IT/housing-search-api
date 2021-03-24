using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using Newtonsoft.Json;

namespace HousingSearchApi.V1.Boundary.Responses
{
    //TODO: Rename to represent to object you will be returning eg. ResidentInformation, HouseholdDetails e.t.c
    public class ResponseObject
    {
        //TODO: add the fields that this API will return here
        //TODO: add xml comments containing information that will be included in the auto generated swagger docs
        //Guidance on XML comments and response objects here (https://github.com/LBHackney-IT/lbh-base-api/wiki/Controllers-and-Response-Objects)
    }

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
