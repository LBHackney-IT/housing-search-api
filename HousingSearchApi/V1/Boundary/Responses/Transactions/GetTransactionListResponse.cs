using System;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Boundary.Responses.Transactions
{
    public class GetTransactionListResponse
    {
        public long Total { get; }
        public List<TransactionResponse> Transactions { get; }

        private GetTransactionListResponse(long total, IEnumerable<TransactionResponse> transactions)
        {
            if (total < 0)
            {
                throw new ArgumentException("Transactions count should be greater that 0.", nameof(total));
            }

            Total = total;
            Transactions = new List<TransactionResponse>(transactions ??
                                                         throw new ArgumentNullException(nameof(transactions), "Transactions cannot be null. Provide empty list if no transactions was found."));
        }

        public static GetTransactionListResponse Create(long total, IEnumerable<TransactionResponse> transactions)
        {
            return new GetTransactionListResponse(total, transactions);
        }
    }
}
