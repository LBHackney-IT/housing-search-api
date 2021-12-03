using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Interfaces;

namespace HousingSearchApi.V1.UseCase
{
    public class GetAccountListUseCase : IGetAccountListUseCase
    {
        private readonly IGetAccountGateway _getAccountGateway;

        public GetAccountListUseCase(IGetAccountGateway getAccountGateway)
        {
            _getAccountGateway = getAccountGateway;
        }

        public async Task<GetAccountListResponse> ExecuteAsync(GetAccountListRequest getAccountListRequest)
        {
            return await _getAccountGateway.Search(getAccountListRequest).ConfigureAwait(false);
        }
    }
}
