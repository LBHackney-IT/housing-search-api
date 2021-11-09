using System.Collections.Generic;
using Hackney.Shared.HousingSearch.Domain.Accounts;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetAccountListResponse
    {
        private long _total;

        public List<Account> Accounts { get; set; }

        public GetAccountListResponse()
        {
            Accounts = new List<Account>();
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
