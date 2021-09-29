using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.Asset;
using HousingSearchApi.V1.Gateways.Models.Assets;
using HousingSearchApi.V1.Gateways.Models.Persons;
using Nest;
using Xunit.Abstractions;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class AssetFixture : BaseFixture
    {
        public List<QueryablePerson> Persons { get; private set; }
        private const string INDEX = "assets";
        public static string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };
        private readonly ITestOutputHelper _output;

        public AssetFixture(IElasticClient elasticClient, HttpClient httpClient, ITestOutputHelper output) : base(elasticClient, httpClient)
        {
            _output = output;
            WaitForESInstance();
        }

        public void GivenAnAssetIndexExists()
        {
            ElasticSearchClient.Indices.Delete(INDEX);

            if (!ElasticSearchClient.Indices.Exists(Indices.Index(INDEX)).Exists)
            {
                var assetSettingsDoc = File.ReadAllTextAsync("./data/elasticsearch/assetIndex.json").Result;
                ElasticSearchClient.LowLevel.Indices.CreateAsync<BytesResponse>(INDEX, assetSettingsDoc)
                    .ConfigureAwait(true);

                var assets = CreateAssetData();
                var awaitable = ElasticSearchClient.IndexManyAsync(assets, INDEX).ConfigureAwait(true);

                while (!awaitable.GetAwaiter().IsCompleted)
                {

                }

                Thread.Sleep(5000);
            }
        }

        private List<QueryableAsset> CreateAssetData()
        {
            var listOfAssets = new List<QueryableAsset>();
            var fixture = new Fixture();
            var random = new Random();

            foreach (var assetTypeString in Enum.GetNames(typeof(AssetType)))
            {
                foreach (var value in Alphabet)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var asset = fixture.Create<QueryableAsset>();
                        asset.Id = fixture.Create<Guid>().ToString();
                        asset.AssetAddress.Uprn = value;
                        asset.AssetType = assetTypeString;
                        asset.Tenure.StartOfTenureDate =
                            fixture.Create<DateTime>().ToString(CultureInfo.InvariantCulture);
                        asset.Tenure.EndOfTenureDate =
                            fixture.Create<DateTime>().ToString(CultureInfo.InvariantCulture);
                        asset.Tenure.Id = fixture.Create<Guid>().ToString();

                        listOfAssets.Add(asset);
                    }
                }
            }

            // Add loads more at random
            for (int i = 0; i < 900; i++)
            {
                var asset = fixture.Create<QueryableAsset>();

                var value = Alphabet[random.Next(0, Alphabet.Length)];
                asset.AssetAddress.Uprn = value;

                listOfAssets.Add(asset);
            }

            return listOfAssets;
        }

        public static List<string> PickStringsFromEnum<T>(int count)
        {
            var random = new Random();
            var types = Enum.GetNames(typeof(T));
            var picked = new List<string>();

            for (var i = 0; i < count; i++)
            {
                // The types selected will _not_ be distinct.
                var randomType = (string) types.GetValue(random.Next(types.Length));
                picked.Add(randomType);
            }

            return picked;
        }
    }
}
