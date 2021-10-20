using System.Collections.Generic;
using System.Threading.Tasks;
using Hackney.Shared.HousingSearch.Domain.Person;

namespace HousingSearchApi.V1.Gateways
{
    public interface IGetPersonGateway
    {
        Task<List<Person>> Search(SearchParameters parameters);
    }
}
