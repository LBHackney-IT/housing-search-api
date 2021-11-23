using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;

namespace HousingSearchApi.V1.Gateways.Interfaces
{
    public interface IGetAccountGateway
    {
        Task<GetAccountListResponse> Search(HousingSearchRequest parameters);
    }
}
