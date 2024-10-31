using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using HousingSearchApi.Tests.V1.TestHelpers;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class TenureFixture : BaseFixture
    {
        public List<QueryablePerson> Persons { get; private set; }
        private const string INDEX = "tenures-test";
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

            foreach (var value in Alphabet)
            {
                for (int i = 0; i < 10; i++)
                {
                    //using model helper instead of Fixture directly to get some custom default values in place
                    var tenure = QueryableTenureHelper.CreateQueyableTenure();
                    tenure.PaymentReference = value;

                    listOfTenures.Add(tenure);
                }
            }

            // Add loads more at random
            for (int i = 0; i < 900; i++)
            {
                var tenure = QueryableTenureHelper.CreateQueyableTenure();

                listOfTenures.Add(tenure);
            }

            return listOfTenures;
        }

        public void GivenSimilarTenures(string paymentReference, string fullAddress, string fullName)
        {
            var listOfTenures = new List<QueryableTenure>();

            var firstTenure = QueryableTenureHelper.CreateQueyableTenure();
            firstTenure.PaymentReference = paymentReference;
            firstTenure.TenuredAsset.FullAddress = fullAddress;
            firstTenure.HouseholdMembers.First().FullName = fullName;

            listOfTenures.Add(firstTenure);

            var secondTenure = QueryableTenureHelper.CreateQueyableTenure();
            secondTenure.PaymentReference = paymentReference;
            listOfTenures.Add(secondTenure);

            var thirdTenure = QueryableTenureHelper.CreateQueyableTenure();
            thirdTenure.TenuredAsset.FullAddress = fullAddress;
            listOfTenures.Add(thirdTenure);

            var forthTenure = QueryableTenureHelper.CreateQueyableTenure();
            thirdTenure.HouseholdMembers.First().FullName = fullName;
            listOfTenures.Add(forthTenure);

            var awaitable = ElasticSearchClient.IndexManyAsync(listOfTenures, INDEX).ConfigureAwait(true);

            while (!awaitable.GetAwaiter().IsCompleted) { }

            Thread.Sleep(10000);
        }

        public void GivenATenureWithSpecificUprn(string uprn)
        {
            var listOfTenures = new List<QueryableTenure>();

            var tenure = QueryableTenureHelper.CreateQueyableTenure();
            tenure.TenuredAsset.Uprn = uprn;

            listOfTenures.Add(tenure);

            var awaitable = ElasticSearchClient.IndexManyAsync(listOfTenures, INDEX).ConfigureAwait(true);

            while (!awaitable.GetAwaiter().IsCompleted) { }

            Thread.Sleep(10000);
        }

        public void GivenTaTenuresExist(int tenuresToCreate)
        {
            var listOfTenures = new List<QueryableTenure>();
            for (var i = 0; i < tenuresToCreate; i++)
            {
                var tenure = QueryableTenureHelper.CreateQueyableTenure();
                tenure.TenuredAsset.IsTemporaryAccommodation = true;

                listOfTenures.Add(tenure);
            }

            var awaitable = ElasticSearchClient.IndexManyAsync(listOfTenures, INDEX).ConfigureAwait(true);

            while (!awaitable.GetAwaiter().IsCompleted) { }

            Thread.Sleep(10000);
        }

        public void GivenSimilarTaTenuresExist(string bookingStatus, string fullName)
        {
            var listOfTenures = new List<QueryableTenure>();

            var firstTenure = QueryableTenureHelper.CreateQueyableTenure();
            firstTenure.TenuredAsset.IsTemporaryAccommodation = true;
            firstTenure.TempAccommodationInfo.BookingStatus = bookingStatus;

            listOfTenures.Add(firstTenure);

            var secondTenure = QueryableTenureHelper.CreateQueyableTenure();
            secondTenure.TenuredAsset.IsTemporaryAccommodation = true;
            secondTenure.HouseholdMembers.First().FullName = fullName;
            listOfTenures.Add(secondTenure);

            var thirdTenure = QueryableTenureHelper.CreateQueyableTenure();
            thirdTenure.TenuredAsset.IsTemporaryAccommodation = true;
            thirdTenure.TempAccommodationInfo.BookingStatus = bookingStatus;
            thirdTenure.HouseholdMembers.First().FullName = fullName;
            listOfTenures.Add(thirdTenure);

            var awaitable = ElasticSearchClient.IndexManyAsync(listOfTenures, INDEX).ConfigureAwait(true);

            while (!awaitable.GetAwaiter().IsCompleted) { }

            Thread.Sleep(10000);
        }

        public void GivenTenuresWithDifferentStartDatesExist()
        {
            //nothing to do here, we already have existing records created by GivenATenureIndexExists fixture
        }

        public void GivenTenuresWithSpecificContentExist(string oldestRecord, string middleRecord, string latestRecord)
        {
            var listOfTenures = new List<QueryableTenure>();

            var latestTenure = QueryableTenureHelper.CreateQueyableTenure();
            latestTenure.StartOfTenureDate = "2024-05-28T15:08:09Z";
            latestTenure.PaymentReference = "veryspecificpaymentreferencefortestinglasthitidone";
            latestTenure.Id = latestRecord;
            listOfTenures.Add(latestTenure);

            var middleTenure = QueryableTenureHelper.CreateQueyableTenure();
            middleTenure.StartOfTenureDate = "2024-05-27T15:08:09Z";
            middleTenure.PaymentReference = "veryspecificpaymentreferencefortestinglasthitidtwo";
            middleTenure.Id = middleRecord;
            listOfTenures.Add(middleTenure);

            var oldestTenure = QueryableTenureHelper.CreateQueyableTenure();
            oldestTenure.StartOfTenureDate = "2024-05-26T15:08:09Z";
            oldestTenure.PaymentReference = "veryspecificpaymentreferencefortestinglasthitidthree";
            oldestTenure.Id = oldestRecord;
            listOfTenures.Add(oldestTenure);

            var awaitable = ElasticSearchClient.IndexManyAsync(listOfTenures, INDEX).ConfigureAwait(true);

            while (!awaitable.GetAwaiter().IsCompleted) { }

            Thread.Sleep(10000);
        }
    }
}
