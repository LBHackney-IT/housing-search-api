using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousingSearchApi.Tests.V1.E2ETests.Steps.ResponseModels
{
    public class TransactionListDTO
    {
        public long Total { get; }
        public List<TransactionDTO> Transactions { get; set; }
    }
}
