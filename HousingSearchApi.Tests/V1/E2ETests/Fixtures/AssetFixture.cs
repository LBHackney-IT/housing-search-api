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
    public class AssetFixture : BaseFixture
    {
        public List<QueryablePerson> Persons { get; private set; }
        private const string INDEX = "assets";
        public static string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

        public AssetFixture(IElasticClient elasticClient, HttpClient httpClient) : base(elasticClient, httpClient)
        {
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

            foreach (var value in Alphabet)
            {
                for (int i = 0; i < 10; i++)
                {
                    var asset = fixture.Create<QueryableAsset>();
                    asset.AssetAddress.Uprn = value;

                    listOfAssets.Add(asset);
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
    }
}
