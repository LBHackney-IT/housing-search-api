using System.Collections.Generic;

namespace HousingSearchApi.V1.Boundary.Responses.Transactions
{
    public class GetTransactionListResponse
    {
        public long Total { get; }
        public List<TransactionResponse> Transactions { get; }

        private GetTransactionListResponse(long total, IEnumerable<TransactionResponse> transactions)
        {
            Total = total;
            Transactions = new List<TransactionResponse>(transactions);
        }

        public static GetTransactionListResponse Create(long total, IEnumerable<TransactionResponse> transactions)
        {
            return new GetTransactionListResponse(total, transactions);
        }
    }
}
