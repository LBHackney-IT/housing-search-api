using LBH_search_api.V1.Boundary.Response;
using LBH_search_api.V1.Factories;
using LBH_search_api.V1.Gateways;
using LBH_search_api.V1.UseCase.Interfaces;

namespace LBH_search_api.V1.UseCase
{
    //TODO: Rename class name and interface name to reflect the entity they are representing eg. GetAllClaimantsUseCase
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly IExampleGateway _gateway;
        public GetAllUseCase(IExampleGateway gateway)
        {
            _gateway = gateway;
        }

        public ResponseObjectList Execute()
        {
            return new ResponseObjectList { ResponseObjects = _gateway.GetAll().ToResponse() };
        }
    }
}
