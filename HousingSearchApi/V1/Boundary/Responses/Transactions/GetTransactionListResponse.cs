using System.Collections.Generic;

namespace HousingSearchApi.V1.Boundary.Responses.Transactions
{
    public class GetTransactionListResponse
    {
        private long _total;

        private readonly List<TransactionResponse> _transactions;

        private GetTransactionListResponse(long total, List<TransactionResponse> transactions)
        {
            _total = total;
            _transactions = new List<TransactionResponse>(transactions);
        }

        public static GetTransactionListResponse Create(long total, List<TransactionResponse> transactions)
        {
            return new GetTransactionListResponse(total, transactions);
        }

        public long Total()
        {
            return _total;
        }
    }
}
