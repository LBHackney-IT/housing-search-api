using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using Nest;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class AccountFixture : BaseFixture
    {
        public static AccountSearchStub[] AccountSearchStubs =
        {
            new AccountSearchStub
            {
                FullAddress = new Fixture().Create<string>(),
                PaymentReference = "12345",
                TargetId = Guid.NewGuid(),
                PrimaryTenants = new List<PrimaryTenants>
                {
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()}
                }
            },
            new AccountSearchStub
            {
                FullAddress = new Fixture().Create<string>(),
                PaymentReference = "12345",
                TargetId = Guid.NewGuid(),
                PrimaryTenants = new List<PrimaryTenants>
                {
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()}
                }
            },
            new AccountSearchStub
            {
                FullAddress = new Fixture().Create<string>(),
                PaymentReference = "12345",
                TargetId = Guid.NewGuid(),
                PrimaryTenants = new List<PrimaryTenants>
                {
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()}
                }
            },
            new AccountSearchStub
            {
                FullAddress = new Fixture().Create<string>(),
                PaymentReference = "12345",
                TargetId = Guid.NewGuid(),
                PrimaryTenants = new List<PrimaryTenants>
                {
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()}
                }
            },
            new AccountSearchStub
            {
                FullAddress = new Fixture().Create<string>(),
                PaymentReference = "12345",
                TargetId = Guid.NewGuid(),
                PrimaryTenants = new List<PrimaryTenants>
                {
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()}
                }
            },
            new AccountSearchStub
            {
                FullAddress = new Fixture().Create<string>(),
                PaymentReference = "12345",
                TargetId = Guid.NewGuid(),
                PrimaryTenants = new List<PrimaryTenants>
                {
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()}
                }
            },
            new AccountSearchStub
            {
                FullAddress = new Fixture().Create<string>(),
                PaymentReference = "12345",
                TargetId = Guid.NewGuid(),
                PrimaryTenants = new List<PrimaryTenants>
                {
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()}
                }
            },
            new AccountSearchStub
            {
                FullAddress = new Fixture().Create<string>(),
                PaymentReference = "12345",
                TargetId = Guid.NewGuid(),
                PrimaryTenants = new List<PrimaryTenants>
                {
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()}
                }
            },
            new AccountSearchStub
            {
                FullAddress = new Fixture().Create<string>(),
                PaymentReference = "12345",
                TargetId = Guid.NewGuid(),
                PrimaryTenants = new List<PrimaryTenants>
                {
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()}
                }
            },
            new AccountSearchStub
            {
                FullAddress = new Fixture().Create<string>(),
                PaymentReference = "12345",
                TargetId = Guid.NewGuid(),
                PrimaryTenants = new List<PrimaryTenants>
                {
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()},
                    new PrimaryTenants() {Id = Guid.NewGuid(), FullName = new Fixture().Create<string>()}
                }
            }
        };

        public AccountFixture(IElasticClient elasticClient, HttpClient httpClient) : base(elasticClient, httpClient)
        {
            INDEX = "accounts";
            WaitForESInstance();
        }

        public void GivenAnAccountIndexExists()
        {
            ElasticSearchClient.Indices.Delete(INDEX);

            if (!ElasticSearchClient.Indices.Exists(Indices.Index(INDEX)).Exists)
            {
                var accountSettingsDoc = File.ReadAllTextAsync("./data/elasticsearch/accountIndex.json").Result;
                ElasticSearchClient.LowLevel.Indices.CreateAsync<BytesResponse>(INDEX, accountSettingsDoc)
                    .ConfigureAwait(true);

                var accounts = CreateAccountData();
                var awaitable = ElasticSearchClient.IndexManyAsync(accounts, INDEX).ConfigureAwait(true);

                while (!awaitable.GetAwaiter().IsCompleted)
                {

                }

                Thread.Sleep(5000);
            }
        }

        private List<QueryableAccount> CreateAccountData()
        {
            var listOfAccounts = new List<QueryableAccount>(4);
            Fixture fixture = new Fixture();

            foreach (var value in AccountSearchStubs)
            {
                var account = fixture.Create<QueryableAccount>();

                account.Tenure.PrimaryTenants = value.PrimaryTenants.Select(p =>
                    new QueryablePrimaryTenant
                    {
                        Id = p.Id,
                        FullName = p.FullName
                    }).ToList<QueryablePrimaryTenant>();
                account.TargetId = value.TargetId;
                account.PaymentReference = value.PaymentReference;
                account.Tenure.FullAddress = value.FullAddress;

                listOfAccounts.Add(account);
            }

            return listOfAccounts;
        }
    }

    public class PrimaryTenants
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
    }
    public class AccountSearchStub
    {
        public Guid TargetId { get; set; }
        public string PaymentReference { get; set; }
        public string FullAddress { get; set; }
        public List<PrimaryTenants> PrimaryTenants { get; set; }
    }
}