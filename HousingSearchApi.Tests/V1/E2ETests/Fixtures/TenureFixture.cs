using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
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

        public void GivenTaTenuresExist(int tenuresToCreate)
        {
            var listOfTenures = new List<QueryableTenure>();
            var fixture = new Fixture();
            for (var i = 0; i < tenuresToCreate; i++)
            {
                var tenure = fixture.Create<QueryableTenure>();
                tenure.TenuredAsset.IsTemporaryAccommodation = true;

                listOfTenures.Add(tenure);
            }

            var awaitable = ElasticSearchClient.IndexManyAsync(listOfTenures, INDEX).ConfigureAwait(true);

            while (!awaitable.GetAwaiter().IsCompleted) { }

            Thread.Sleep(10000);
        }

        public void GivenSimilarTaTenuresExist(string bookingStatus, string fullName)
        {
            var fixture = new Fixture();
            var listOfTenures = new List<QueryableTenure>();

            var firstTenure = fixture.Create<QueryableTenure>();
            firstTenure.TenuredAsset.IsTemporaryAccommodation = true;
            firstTenure.TempAccommodationInfo.BookingStatus = bookingStatus;

            listOfTenures.Add(firstTenure);

            var secondTenure = fixture.Create<QueryableTenure>();
            secondTenure.TenuredAsset.IsTemporaryAccommodation = true;
            secondTenure.HouseholdMembers.First().FullName = fullName;
            listOfTenures.Add(secondTenure);

            var thirdTenure = fixture.Create<QueryableTenure>();
            thirdTenure.TenuredAsset.IsTemporaryAccommodation = true;
            thirdTenure.TempAccommodationInfo.BookingStatus = bookingStatus;
            thirdTenure.HouseholdMembers.First().FullName = fullName;
            listOfTenures.Add(thirdTenure);

            var awaitable = ElasticSearchClient.IndexManyAsync(listOfTenures, INDEX).ConfigureAwait(true);

            while (!awaitable.GetAwaiter().IsCompleted) { }

            Thread.Sleep(10000);
        }
    }
}
