using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Newtonsoft.Json;
using SearchApi.V1.Boundary.Responses.Metadata;

namespace SearchApi.V1.Boundary.Responses
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

        [JsonProperty("errors")]
        public IEnumerable<Error> Errors { get; set; }

        public ErrorResponse() { }

        public ErrorResponse(ValidationResult validationResult)
        {
            var errors = validationResult.Errors
                .Select(validationResultError => new Error(validationResultError)).ToList();
            Errors = errors;
            StatusCode = 400;
        }

        public ErrorResponse(int statusCode, IEnumerable<Error> errors)
        {
            Errors = errors;
            StatusCode = statusCode;
        }
    }
}
