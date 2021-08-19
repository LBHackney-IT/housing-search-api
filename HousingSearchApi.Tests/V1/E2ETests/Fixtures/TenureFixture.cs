using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using HousingSearchApi.V1.Gateways.Models;
using Nest;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class TenureFixture : BaseFixture
    {
        public List<QueryablePerson> Persons { get; private set; }
        private const string INDEX = "tenures";

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
                ElasticSearchClient.IndexManyAsync(tenures, INDEX);

                var timeout = DateTime.UtcNow.AddSeconds(10); // 10 second timeout to make sure all the data is there.

                while (DateTime.UtcNow < timeout)
                {
                    var count = ElasticSearchClient.Cluster.Stats().Indices.Documents.Count;
                    if (count >= tenures.Count)
                        break;

                    Thread.Sleep(200);
                }
            }
        }

        private List<QueryableTenure> CreateTenureData()
        {
            var listOfTenures = new List<QueryableTenure>();
            var random = new Random();
            var fixture = new Fixture();

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
