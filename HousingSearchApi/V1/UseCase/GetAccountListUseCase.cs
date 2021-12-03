using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Interfaces;

namespace HousingSearchApi.V1.UseCase
{
    public class GetAccountListUseCase : IGetAccountListUseCase
    {
        private readonly ISearchGateway _gateway;

        public GetAccountListUseCase(ISearchGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<GetAccountListResponse> ExecuteAsync(GetAccountListRequest getAccountListRequest)
        {
            return await _gateway.GetListOfAccounts(getAccountListRequest).ConfigureAwait(false);
        }
    }
}
