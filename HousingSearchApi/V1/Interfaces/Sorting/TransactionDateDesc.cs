using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using Nest;

namespace HousingSearchApi.V1.Interfaces.Sorting
{
    public class TransactionDateDesc : ISort<QueryableTransaction>
    {
        public SortDescriptor<QueryableTransaction> GetSortDescriptor(SortDescriptor<QueryableTransaction> descriptor)
        {
            return descriptor
                .Descending(f => f.TransactionDate);
        }
    }
}
