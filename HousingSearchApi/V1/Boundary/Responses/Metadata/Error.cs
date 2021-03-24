using FluentValidation.Results;

namespace HousingSearchApi.V1.Boundary.Responses.Metadata
{
    public class Error
    {
        public string Message { get; set; }
        public string FieldName { get; set; }

        public Error() { }

        public Error(ValidationFailure validationFailure)
        {
            Message = validationFailure?.ErrorMessage;
            FieldName = validationFailure?.PropertyName;
        }

        public Error(string message, string fieldName = null)
        {
            Message = message;
            FieldName = fieldName;
        }
    }
}
