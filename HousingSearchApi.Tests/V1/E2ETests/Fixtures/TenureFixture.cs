using AutoFixture;
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
        private const string INDEX = "tenures";
        public static string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

        // Please note Fixture customization will be ignored altogether if Build() is used. Customization only applies to .Create(). This is by design in AutoFixture itself
        // If using .Build() in tests ensure the StartOfTenureDate is handled appropriately, so it can be used with date field type in the index

        // Fixture Customizations run only once per Fixture instance, so that's why we have to use a new instance every time we create a new QueryableTenure object
        //  This ensures we have a proper range of valid dates for tenure start dates

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
                    var tenure = new Fixture().Customize(new CreateRandomISOStartOfTenureDate()).Create<QueryableTenure>();
                    tenure.PaymentReference = value;

                    listOfTenures.Add(tenure);
                }
            }

            // Add loads more at random
            for (int i = 0; i < 900; i++)
            {
                var tenure = new Fixture().Customize(new CreateRandomISOStartOfTenureDate()).Create<QueryableTenure>();

                listOfTenures.Add(tenure);
            }

            return listOfTenures;
        }

        public void GivenSimilarTenures(string paymentReference, string fullAddress, string fullName)
        {
            var listOfTenures = new List<QueryableTenure>();

            var firstTenure = new Fixture().Customize(new CreateRandomISOStartOfTenureDate()).Create<QueryableTenure>();
            firstTenure.PaymentReference = paymentReference;
            firstTenure.TenuredAsset.FullAddress = fullAddress;
            firstTenure.HouseholdMembers.First().FullName = fullName;

            listOfTenures.Add(firstTenure);

            var secondTenure = new Fixture().Customize(new CreateRandomISOStartOfTenureDate()).Create<QueryableTenure>();
            secondTenure.PaymentReference = paymentReference;
            listOfTenures.Add(secondTenure);

            var thirdTenure = new Fixture().Customize(new CreateRandomISOStartOfTenureDate()).Create<QueryableTenure>();
            thirdTenure.TenuredAsset.FullAddress = fullAddress;
            listOfTenures.Add(thirdTenure);

            var forthTenure = new Fixture().Customize(new CreateRandomISOStartOfTenureDate()).Create<QueryableTenure>();
            thirdTenure.HouseholdMembers.First().FullName = fullName;
            listOfTenures.Add(forthTenure);

            var awaitable = ElasticSearchClient.IndexManyAsync(listOfTenures, INDEX).ConfigureAwait(true);

            while (!awaitable.GetAwaiter().IsCompleted) { }

            Thread.Sleep(10000);
        }

        public void GivenATenureWithSpecificUprn(string uprn)
        {
            var listOfTenures = new List<QueryableTenure>();

            var tenure = new Fixture().Customize(new CreateRandomISOStartOfTenureDate()).Create<QueryableTenure>();
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
                var tenure = new Fixture().Customize(new CreateRandomISOStartOfTenureDate()).Create<QueryableTenure>();
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

            var firstTenure = new Fixture().Customize(new CreateRandomISOStartOfTenureDate()).Create<QueryableTenure>();
            firstTenure.TenuredAsset.IsTemporaryAccommodation = true;
            firstTenure.TempAccommodationInfo.BookingStatus = bookingStatus;

            listOfTenures.Add(firstTenure);

            var secondTenure = new Fixture().Customize(new CreateRandomISOStartOfTenureDate()).Create<QueryableTenure>();
            secondTenure.TenuredAsset.IsTemporaryAccommodation = true;
            secondTenure.HouseholdMembers.First().FullName = fullName;
            listOfTenures.Add(secondTenure);

            var thirdTenure = new Fixture().Customize(new CreateRandomISOStartOfTenureDate()).Create<QueryableTenure>();
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
    }
}
