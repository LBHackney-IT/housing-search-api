using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackney.Shared.HousingSearch.Domain.Accounts;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps.ResponseModels
{
    public class AccountListDTO
    {
        public List<QueryableAccount> Accounts { get; set; }
        public long Total { get; set; }
    }
}
