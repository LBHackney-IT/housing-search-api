using System.Collections.Generic;
using SearchApi.V1.Domain;

namespace SearchApi.V1.Gateways
{
    public interface IExampleGateway
    {
        Entity GetEntityById(int id);

        List<Entity> GetAll();
    }
}
