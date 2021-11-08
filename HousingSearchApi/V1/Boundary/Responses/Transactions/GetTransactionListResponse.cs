using System.Collections.Generic;
using Hackney.Shared.HousingSearch.Domain.Transactions;

namespace HousingSearchApi.V1.Boundary.Responses.Transactions
{
    public class GetTransactionListResponse
    {
        private long _total;

        public List<Transaction> Transactions { get; set; }

        public GetTransactionListResponse()
        {
            Transactions = new List<Transaction>();
        }

        public void SetTotal(long total)
        {
            _total = total;
        }

        public long Total()
        {
            return _total;
        }


    }
}
