using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using Nest;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class TenureFixture : BaseFixture
    {
        public static string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

        public TenureFixture(IElasticClient elasticClient, HttpClient httpClient) : base(elasticClient, httpClient)
        {
            INDEX = "tenures";
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

                Thread.Sleep(1000);
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

        public void GivenSimilarTenures(string paymentReference, string fullAddress, string fullName)
        {
            var fixture = new Fixture();
            var listOfTenures = new List<QueryableTenure>();

            var firstTenure = fixture.Create<QueryableTenure>();
            firstTenure.PaymentReference = paymentReference;
            firstTenure.TenuredAsset.FullAddress = fullAddress;
            firstTenure.HouseholdMembers.First().FullName = fullName;

            listOfTenures.Add(firstTenure);

            var secondTenure = fixture.Create<QueryableTenure>();
            secondTenure.PaymentReference = paymentReference;
            listOfTenures.Add(secondTenure);

            var thirdTenure = fixture.Create<QueryableTenure>();
            thirdTenure.TenuredAsset.FullAddress = fullAddress;
            listOfTenures.Add(thirdTenure);

            var forthTenure = fixture.Create<QueryableTenure>();
            thirdTenure.HouseholdMembers.First().FullName = fullName;
            listOfTenures.Add(forthTenure);

            var awaitable = ElasticSearchClient.IndexManyAsync(listOfTenures, INDEX).ConfigureAwait(true);

            while (!awaitable.GetAwaiter().IsCompleted) { }

            Thread.Sleep(10000);
        }
    }
}
