using System.Collections.Generic;

namespace HousingSearchApi.V1.Boundary.Responses.Metadata
{
    public class APIError
    {
        public bool IsValid { get; set; }
        public IList<ValidationError> ValidationErrors { get; set; }
    }
}
