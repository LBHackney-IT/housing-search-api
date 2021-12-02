using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Domain.Accounts;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using Nest;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class AccountFixture : BaseFixture
    {
        private readonly QueryablePrimaryTenant[] _primaryTenants =
        {
            new QueryablePrimaryTenant(){FullName = new Fixture().Create<string>(),Id = Guid.NewGuid()},
            new QueryablePrimaryTenant(){FullName = new Fixture().Create<string>(),Id = Guid.NewGuid()},
            new QueryablePrimaryTenant(){FullName = new Fixture().Create<string>(),Id = Guid.NewGuid()},
            new QueryablePrimaryTenant(){FullName = new Fixture().Create<string>(),Id = Guid.NewGuid()},
            new QueryablePrimaryTenant(){FullName = new Fixture().Create<string>(),Id = Guid.NewGuid()}
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
            var listOfAccounts = new List<QueryableAccount>();
            Fixture fixture = new Fixture();

            foreach (var value in _primaryTenants)
            {
                var account = fixture.Create<QueryableAccount>();
                account.Tenure.PrimaryTenants.Add(value);

                listOfAccounts.Add(account);
            }

            return listOfAccounts;
        }
    }
}
