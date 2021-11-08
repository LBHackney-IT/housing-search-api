using System.Text.Json.Serialization;

namespace HousingSearchApi.V1.Boundary.Responses
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        Tenure
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionType
    {
        Rent,
        Charge
    }
}
