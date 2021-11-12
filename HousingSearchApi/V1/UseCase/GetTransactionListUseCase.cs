using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses.Transactions;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.UseCase
{
    public class GetTransactionListUseCase : IGetTransactionListUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public GetTransactionListUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        [LogCall]
        public async Task<GetTransactionListResponse> ExecuteAsync(GetTransactionSearchRequest getTransactionSearchRequest)
        {
            return await _searchGateway.GetListOfTransactions(getTransactionSearchRequest).ConfigureAwait(false);
        }
    }
}
