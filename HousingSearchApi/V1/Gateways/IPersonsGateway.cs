using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.Gateways
{
    public interface IPersonsGateway
    {
        GetPersonListResponse GetListOfPersons(GetPersonListRequest getPersonListRequest);
    }
}
