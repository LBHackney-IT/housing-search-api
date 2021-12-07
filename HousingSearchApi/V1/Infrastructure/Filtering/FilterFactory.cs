using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Interfaces.Filtering;

namespace HousingSearchApi.V1.Infrastructure.Filtering
{
    public class FilterFactory : IFilterFactory
    {
        public IFilter<T> Create<T, TRequest>(TRequest request) where T : class where TRequest : class
        {
            if (typeof(T) == typeof(QueryableTransaction))
            {
                return (IFilter<T>) new TransactionDateRange();
            }

            return new DefaultFilter<T>();
        }
    }
}
