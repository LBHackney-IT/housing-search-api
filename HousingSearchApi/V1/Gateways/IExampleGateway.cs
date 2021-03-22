using System.Collections.Generic;
using HousingSearchApi.V1.Domain;

namespace HousingSearchApi.V1.Gateways
{
    public interface IExampleGateway
    {
        Entity GetEntityById(int id);

        List<Entity> GetAll();
    }
}
