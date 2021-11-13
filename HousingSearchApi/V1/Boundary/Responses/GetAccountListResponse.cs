using System.Collections.Generic;
using Hackney.Shared.HousingSearch.Domain.Accounts;

namespace HousingSearchApi.V1.Boundary.Responses
{
    public class GetAccountListResponse
    {
        public static GetAccountListResponse Create(List<Account> accounts)
        {
            return new GetAccountListResponse(accounts);
        }

        private GetAccountListResponse(List<Account> accounts)
        {
            Accounts = accounts;
        }

        private long _total;

        public List<Account> Accounts { get; }

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
