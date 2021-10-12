using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using HousingSearchApi.V1.Gateways.Models.Assets;
using HousingSearchApi.V1.Gateways.Models.Persons;
using Nest;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class AssetFixture : BaseFixture
    {
        public List<QueryablePerson> Persons { get; private set; }
        private const string INDEX = "assets";
        public static AddressStub[] Addresses =
        {
            new AddressStub{ FistLine = "G 1 Something Street", AssetType = "FirstAsset"},
            new AddressStub{ FistLine = "G 11 Something Street", AssetType = "FirstAsset"},
            new AddressStub{ FistLine = "123 Something Street", AssetType = "SecondAsset"},
            new AddressStub{ FistLine = "1 Something street", AssetType = "FirstAsset"},
            new AddressStub{ FistLine = "11 Something street", AssetType = "FirstAsset"},
            new AddressStub{ FistLine = "1111 Something street", AssetType = "FirstAsset"},
            new AddressStub{ FistLine = "100 Something street", AssetType = "ThirdAsset"},
            new AddressStub{ FistLine = "21 Something street", AssetType = "FirstAsset"},
            new AddressStub{ FistLine = "G 12 Something Street", AssetType = "SecondAsset"},
            new AddressStub{ FistLine = "2123 Something Street", AssetType = "ThirdAsset"},
            new AddressStub{ FistLine = "200 Something street", AssetType = "SecondAsset"}
        };

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

            foreach (var value in Addresses)
            {
                var asset = fixture.Create<QueryableAsset>();
                asset.AssetAddress.AddressLine1 = value.FistLine;
                asset.AssetType = value.AssetType;

                listOfAssets.Add(asset);
            }

            return listOfAssets;
        }
    }

    public class AddressStub
    {
        public string FistLine { get; set; }
        public string AssetType { get; set; }
    }
}
