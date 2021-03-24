using SearchApi.V1.Domain;

namespace SearchApi.V1.Gateways
{
    public interface IPersonsGateway
    {
        GetPersonListResponse GetListOfPersons(GetPersonListRequest getPersonListRequest);
    }
}
