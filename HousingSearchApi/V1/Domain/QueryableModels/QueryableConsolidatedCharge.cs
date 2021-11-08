using Nest;

namespace HousingSearchApi.V1.Domain.QueryableModels
{
    public class QueryableConsolidatedCharge
    {
        [Text(Name = "type")]
        public string Type { get; set; }

        [Text(Name = "frequency")]
        public string Frequency { get; set; }

        [Text(Name = "amount")]
        public decimal Amount { get; set; }
    }
}
