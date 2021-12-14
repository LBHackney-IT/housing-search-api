using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using Nest;
using System;

namespace HousingSearchApi.V1.Interfaces.Filtering
{
    public class TransactionsFilter : IFilter<QueryableTransaction>
    {
        /// <summary>
        /// Method to perform filtering from start to end dates
        /// </summary>
        public QueryContainerDescriptor<QueryableTransaction> GetDescriptor<TRequest>(QueryContainerDescriptor<QueryableTransaction> descriptor, TRequest request)
        {
            if (!(request is GetTransactionListRequest transactionSearchRequest))
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (transactionSearchRequest.StartDate.HasValue ||
                transactionSearchRequest.EndDate.HasValue)
            {
                var (startDate, endDate) = GetSearchDateRange(transactionSearchRequest);

                return (QueryContainerDescriptor<QueryableTransaction>) descriptor.DateRange(c => c
                    .Field(p => p.CreatedAt)
                    .GreaterThanOrEquals(startDate)
                    .LessThanOrEquals(endDate));
            }

            return null;
        }

        private static (DateTime startDate, DateTime endDate) GetSearchDateRange(GetTransactionListRequest request)
        {
            var startDate = request.StartDate == null ? DateTime.UnixEpoch : (DateTime) request.StartDate;
            var endDate = request.EndDate == null ? DateTime.UtcNow : (DateTime) request.EndDate;

            return (startDate, endDate);
        }
    }
}
