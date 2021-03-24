using System;
using FluentValidation.Results;

namespace HousingSearchApi.V1.Boundary.Responses.Metadata
{
    public class BadRequestException : Exception
    {
        public ValidationResult ValidationResponse { get; set; }

        public BadRequestException() : base("Request is null")
        { }
        public BadRequestException(string message) : base(message)
        { }

        public BadRequestException(ValidationResult validationResponse)
        {
            ValidationResponse = validationResponse;
        }
    }
}
