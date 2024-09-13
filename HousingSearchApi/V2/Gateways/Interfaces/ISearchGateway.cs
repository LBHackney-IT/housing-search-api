using System.Collections.Generic;
using System.Threading.Tasks;

namespace HousingSearchApi.V2.Gateways.Interfaces;

public interface ISearchGateway
{
    Task<IReadOnlyCollection<object>> FreeSearch(string indexName, string searchText);
}
