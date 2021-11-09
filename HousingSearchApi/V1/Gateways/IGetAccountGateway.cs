using System.Collections.Generic;
using System.Threading.Tasks;
using Hackney.Shared.HousingSearch.Domain.Accounts;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;

namespace HousingSearchApi.V1.Gateways
{
    public interface IGetAccountGateway
    {
        Task<GetAccountListResponse> Search(HousingSearchRequest parameters);
    }
}
