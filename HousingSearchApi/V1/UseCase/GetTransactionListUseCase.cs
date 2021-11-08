using Hackney.Core.Logging;
using Hackney.Shared.HousingSearch.Domain.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses.Transactions;
using HousingSearchApi.V1.Interfaces;
using HousingSearchApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
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
        public Task<GetTransactionListResponse> ExecuteAsync(TransactionSearchRequest transactionSearchRequest)
        {
            return Task.FromResult( new GetTransactionListResponse()
            {
                Transactions = new List<Transaction>(1)
                {
                    new Transaction() {Address = "sss", Id = Guid.NewGuid()}
                }
            });
            //return await _searchGateway.GetListOfTransactions(transactionSearchRequest).ConfigureAwait(false);
        }
    }
}
