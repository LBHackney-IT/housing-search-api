using FluentValidation.Results;

namespace SearchApi.V1.Boundary.Responses.Metadata
{
    public class ValidationError
    {
        public string Message { get; set; }
        public string FieldName { get; set; }

        public ValidationError()
        {

        }

        public ValidationError(ValidationFailure validationFailure)
        {
            Message = validationFailure?.ErrorMessage;
            FieldName = validationFailure?.PropertyName;
        }
    }
}
