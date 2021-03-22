using System.Collections.Generic;
using System.Threading.Tasks;
using SearchApi.V1.Domain;

namespace SearchApi.V1.Gateways
{
    public interface IGetPersonGateway
    {
        Task<List<Person>> Search(SearchParameters parameters);
    }
}
