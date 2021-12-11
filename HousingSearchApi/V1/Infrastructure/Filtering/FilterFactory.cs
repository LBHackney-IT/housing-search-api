using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Filtering;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Filtering
{
    public class FilterFactory : IFilterFactory
    {
        /// <summary>
        /// Method to perform filtering from start to end dates
        /// </summary>
        public QueryContainer Filter<T, TRequest>(TRequest request, QueryContainerDescriptor<T> q) where T : class where TRequest : class
        {
            // ToDo: implement factory for choosing type of filter
            if (request is GetTransactionListRequest transactionSearchRequest &&
                q is QueryContainerDescriptor<QueryableTransaction> transactionDescriptor)
            {
                if (transactionSearchRequest.StartDate.HasValue &&
                    transactionSearchRequest.EndDate.HasValue)
                {
                    return transactionDescriptor.DateRange(c => c
                        .Field(p => p.CreatedAt)
                        .GreaterThanOrEquals(transactionSearchRequest.StartDate)
                        .LessThanOrEquals(transactionSearchRequest.EndDate));
                }
            }

            return null;
        }
    }
}
