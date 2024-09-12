using System.Collections.Generic;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Gateways.Interfaces;

public interface IGetGateway
{
    Task<IReadOnlyCollection<object>> FreeSearch(string indexName, string searchText);
}
