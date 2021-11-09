using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
using HousingSearchApi.V1.Domain;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Gateways
{
    interface IGetAccountGateway
    {
        public Task<APIResponse<Account>> SearchAsync(GetAccountListRequest getAccountListRequest);
    }
}
