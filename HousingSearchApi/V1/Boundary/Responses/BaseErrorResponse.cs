using Newtonsoft.Json;
using System.Net;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class BaseErrorResponse
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public BaseErrorResponse(string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
        {
            StatusCode = (int) httpStatusCode;
            Message = message;
        }
    }
}
