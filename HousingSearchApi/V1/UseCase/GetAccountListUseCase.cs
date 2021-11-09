using System.Collections.Generic;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Gateways;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Responses.Metadata;

namespace HousingSearchApi.V1.UseCase
{
    public class GetAccountListUseCase : IGetAccountListUseCase
    {
        private readonly GetAccountGateway _getAccountGateway;

        public GetAccountListUseCase(GetAccountGateway getAccountGateway)
        {
            _getAccountGateway = getAccountGateway;
        }

        public async Task<APIResponse<List<Account>>> ExecuteAsync(GetAccountListRequest getAccountListRequest)
        {
            return await _getAccountGateway.SearchAsync(getAccountListRequest).ConfigureAwait(false);
        }
    }
}
