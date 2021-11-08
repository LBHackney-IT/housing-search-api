using System.Text.Json.Serialization;

namespace HousingSearchApi.V1.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RentGroupType
    {
        Tenant,
        LeaseHolders,
        GenFundRents,
        Garages,
        HaLeases,
        HraRents,
        MajorWorks,
        TempAcc,
        Travelers
    }
}
