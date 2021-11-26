using System;
using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Factories;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Factories
{
    public class TransactionsQueryGenerator : IQueryGenerator<QueryableTransaction>
    {
        private readonly IQueryBuilder<QueryableTransaction> _queryBuilder;

        public TransactionsQueryGenerator(IQueryBuilder<QueryableTransaction> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public QueryContainer Create<TRequest>(TRequest request, QueryContainerDescriptor<QueryableTransaction> q)
        {
            if (!(request is GetTransactionListRequest transactionSearchRequest))
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(transactionSearchRequest.SearchText)) return null;

            return _queryBuilder
                .WithWildstarQuery(transactionSearchRequest.SearchText,
                    new List<string>
                    {
                        "sender.fullName",
                        "transactionType",
                        "paymentReference",
                        "bankAccountNumber",
                        "transactionDate",
                        "transactionAmount"
                    })
                .WithExactQuery(transactionSearchRequest.SearchText,
                    new List<string>
                    {
                        "sender.fullName",
                        "transactionType",
                        "paymentReference",
                        "bankAccountNumber",
                        "transactionDate",
                        "transactionAmount"
                    })
                .Build(q);
        }
    }
}
