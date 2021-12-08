using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Interfaces.Filtering;

namespace HousingSearchApi.V1.Infrastructure.Filtering
{
    public class FilterFactory : IFilterFactory
    {
        /// <summary>
        /// Method to perform filtering from start to end dates
        /// </summary>
        public IFilter<T> Create<T, TRequest>(TRequest request) where T : class where TRequest : class
        {
            if (typeof(T) == typeof(QueryableTransaction))
            {
                return (IFilter<T>) new TransactionsFilter();
            }

            return new DefaultFilter<T>();
        }
    }
}
