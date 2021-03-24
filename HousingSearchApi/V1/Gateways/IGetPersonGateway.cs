using System.Collections.Generic;
using System.Threading.Tasks;
using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.Gateways
{
    public interface IGetPersonGateway
    {
        Task<List<Person>> Search(SearchParameters parameters);
    }
}
