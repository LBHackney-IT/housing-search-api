using System.Collections.Generic;
using System.Threading.Tasks;
using Hackney.Shared.HousingSearch.Domain.Person;
using HousingSearchApi.V1.Gateways.Domain;

namespace HousingSearchApi.V1.Gateways
{
    public interface IGetPersonGateway
    {
        Task<List<Person>> Search(SearchParameters parameters);
    }
}
