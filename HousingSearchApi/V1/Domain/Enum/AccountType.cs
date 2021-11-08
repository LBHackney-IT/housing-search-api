using System.Text.Json.Serialization;

namespace HousingSearchApi.V1.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AccountType
    {
        Master,
        Recharge,
        Sundry
    }
}
