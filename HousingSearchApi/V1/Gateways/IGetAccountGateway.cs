using System.Collections.Generic;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Domain;
using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Responses.Metadata;

namespace HousingSearchApi.V1.Gateways
{
    interface IGetAccountGateway
    {
        public Task<APIResponse<List<Account>>> SearchAsync(GetAccountListRequest getAccountListRequest);
    }
}
