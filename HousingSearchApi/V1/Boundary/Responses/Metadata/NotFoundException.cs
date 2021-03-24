using System;
using FluentValidation.Results;

namespace HousingSearchApi.V1.Boundary.Responses.Metadata
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Result not found")
        { }
        public NotFoundException(string message) : base(message)
        { }
    }
}
