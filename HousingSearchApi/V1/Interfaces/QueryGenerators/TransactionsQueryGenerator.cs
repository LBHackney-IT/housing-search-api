using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;
using HousingSearchApi.V1.Boundary.Requests;
using Nest;

namespace HousingSearchApi.V1.Interfaces.QueryGenerators
{
    public class TransactionsQueryGenerator : IQueryGenerator<QueryableTransaction>
    {
        private readonly IQueryBuilder<QueryableTransaction> _queryBuilder;

        public TransactionsQueryGenerator(IQueryBuilder<QueryableTransaction> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryableTransaction> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;

            return _queryBuilder
                .WithWildstarQuery(request.SearchText,
                    new List<string>
                    {
                        "sender.fullName",
                        "transactionType",
                        "paymentReference",
                        "bankAccountNumber",
                        "transactionDate",
                        "transactionAmount"
                    })
                .WithExactQuery(request.SearchText,
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
