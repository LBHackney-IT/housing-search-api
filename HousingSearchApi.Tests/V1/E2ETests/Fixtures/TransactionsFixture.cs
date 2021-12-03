using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nest;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Gateways.Models.Transactions;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class TransactionsFixture : BaseFixture
    {
        private const string IndexName = "transactions";
        private const int SendersCount = 5;

        private static readonly Fixture _fixture = new Fixture();

        public static List<TransactionSearchStub> TransactionSearchStub { get; } = CreateTransactionSearchStubs(SendersCount);

        public TransactionsFixture(IElasticClient elasticClient, HttpClient httpHttpClient) : base(elasticClient, httpHttpClient)
        {
            WaitForESInstance();
        }

        private static List<TransactionSearchStub> CreateTransactionSearchStubs(int personsCount)
        {
            return _fixture.CreateMany<TransactionSearchStub>(personsCount).ToList();
        }

        public void GivenATransactionIndexExists()
        {
            ElasticSearchClient.Indices.Delete(IndexName);

            if (ElasticSearchClient.Indices.Exists(Indices.Index(IndexName)).Exists)
            {
                return;
            }

            // ToDo: add transactionsIndex to the folder
            var transactionSettingsDoc = File.ReadAllTextAsync("./data/elasticsearch/transactionsIndex.json").Result;
            ElasticSearchClient.LowLevel.Indices.CreateAsync<BytesResponse>(IndexName, transactionSettingsDoc)
                .ConfigureAwait(true);

            var transactions = CreateTransactionsData(20);
            var awaitable = ElasticSearchClient.IndexManyAsync(transactions, IndexName).ConfigureAwait(true);

            while (!awaitable.GetAwaiter().IsCompleted) { }

            Thread.Sleep(5000);
        }

        private List<QueryableTransaction> CreateTransactionsData(int transactionsCount)
        {
            var listOfTransactions = new List<QueryableTransaction>(transactionsCount);
            var random = new Random();

            for (var i = 0; i < transactionsCount; i++)
            {
                var personIndex = random.Next(SendersCount);

                var transaction = _fixture.Create<QueryableTransaction>();
                transaction.Sender = TransactionSearchStub[personIndex].Sender;
                transaction.TargetId = TransactionSearchStub[personIndex].TargetId;

                listOfTransactions.Add(transaction);
            }

            return listOfTransactions;
        }
    }

    public class TransactionSearchStub
    {
        public QueryableSender Sender { get; set; }
        public Guid TargetId { get; set; }
    }
}
