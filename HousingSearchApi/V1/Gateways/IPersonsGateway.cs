using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.Gateways
{
    public interface IPersonsGateway
    {
        GetPersonListResponse GetListOfPersons(GetPersonListRequest getPersonListRequest);
    }

    public class PersonsGateway : IPersonsGateway
    {
        public GetPersonListResponse GetListOfPersons(GetPersonListRequest getPersonListRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}
