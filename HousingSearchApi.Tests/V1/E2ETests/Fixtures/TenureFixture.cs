using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using HousingSearchApi.V1.Gateways.Models.Persons;
using HousingSearchApi.V1.Gateways.Models.Tenures;
using Nest;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class TenureFixture : BaseFixture
    {
        public List<QueryablePerson> Persons { get; private set; }
        private const string INDEX = "tenures";
        public static string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

        public TenureFixture(IElasticClient elasticClient, HttpClient httpClient) : base(elasticClient, httpClient)
        {
            WaitForESInstance();
        }

        public void GivenATenureIndexExists()
        {
            ElasticSearchClient.Indices.Delete(Indices.Index(INDEX));

            if (!ElasticSearchClient.Indices.Exists(Indices.Index(INDEX)).Exists)
            {
                var tenureSettingsDoc = File.ReadAllTextAsync("./data/elasticsearch/tenureIndex.json").Result;
                ElasticSearchClient.LowLevel.Indices.CreateAsync<BytesResponse>(INDEX, tenureSettingsDoc)
                    .ConfigureAwait(true);

                var tenures = CreateTenureData();
                var awaitable = ElasticSearchClient.IndexManyAsync(tenures, INDEX).ConfigureAwait(true);

                while (!awaitable.GetAwaiter().IsCompleted)
                {

                }

                Thread.Sleep(5000);
            }
        }

        private List<QueryableTenure> CreateTenureData()
        {
            var listOfTenures = new List<QueryableTenure>();
            var random = new Random();
            var fixture = new Fixture();

            foreach (var value in Alphabet)
            {
                for (int i = 0; i < 10; i++)
                {
                    var tenure = fixture.Create<QueryableTenure>();
                    tenure.PaymentReference = value;

                    listOfTenures.Add(tenure);
                }
            }

            // Add loads more at random
            for (int i = 0; i < 900; i++)
            {
                var tenure = fixture.Create<QueryableTenure>();

                listOfTenures.Add(tenure);
            }

            return listOfTenures;
        }
    }
}
