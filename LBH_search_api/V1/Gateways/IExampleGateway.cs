using System.Collections.Generic;
using LBH_search_api.V1.Domain;

namespace LBH_search_api.V1.Gateways
{
    public interface IExampleGateway
    {
        Entity GetEntityById(int id);

        List<Entity> GetAll();
    }
}
