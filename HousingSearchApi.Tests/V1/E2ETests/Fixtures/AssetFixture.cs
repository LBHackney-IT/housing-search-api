using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using Nest;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class AssetFixture : BaseFixture
    {
        public List<QueryablePerson> Persons { get; private set; }
        private const string INDEX = "assets";
        public static AddressStub[] Addresses =
        {
            new AddressStub{ FirstLine = "59 Buckland Court St Johns Estate", AssetType = "FirstAsset", PostCode = "N1 5EP", UPRN = "10008234650"},
            new AddressStub{ FirstLine = "54 Buckland Court St Johns Estate", AssetType = "FirstAsset", PostCode = "N1 5EP", UPRN = "10008234655"},
            new AddressStub{ FirstLine = "65 Buckland Court St Johns Estate", AssetType = "SecondAsset", PostCode = "N1 5EP", UPRN = "10008234605"},
            new AddressStub{ FirstLine = "45 Buckland Court St Johns Estate", AssetType = "FirstAsset", PostCode = "N1 5EP", UPRN = "10008234650"},
            new AddressStub{ FirstLine = "Gge 45 Buckland Court St Johns Estate", AssetType = "FirstAsset", PostCode = "N1 5EP", UPRN = "10008234650"},
            new AddressStub{ FirstLine = "Gge 52 Buckland Court St Johns Estate", AssetType = "FirstAsset", PostCode = "N1 5EP", UPRN = "10008234650"},
            new AddressStub{ FirstLine = "Gge 51 Buckland Court St Johns Estate", AssetType = "ThirdAsset", PostCode = "N1 5EP", UPRN = "10008234650"},
            new AddressStub{ FirstLine = "5 Buckland Court St Johns Estate", AssetType = "FirstAsset", PostCode = "N1 6TY", UPRN = "10008235183"},
            new AddressStub{ FirstLine = "Gge 15 Buckland Court St Johns Estate", AssetType = "SecondAsset", PostCode = "N1 5EP", UPRN = "10008234650"},
            new AddressStub{ FirstLine = "Gge 53 Buckland Court St Johns Estate", AssetType = "ThirdAsset", PostCode = "N1 5EP", UPRN = "10008234650"},
            new AddressStub{ FirstLine = "Gge 25 Buckland Court St Johns Estate", AssetType = "SecondAsset", PostCode = "N1 5EP", UPRN = "10008234650"},
            new AddressStub{ FirstLine = "Gge 9 Haslemere Court Holmleigh Road Estate", AssetType = "LettableNonDwelling", PostCode = "N16 5QP", UPRN = ""},
            new AddressStub{ FirstLine = "Gge 28 Blandford Court St Peters Way", AssetType = "LettableNonDwelling", PostCode = "N1 4SA", UPRN = ""},
            new AddressStub{ FirstLine = "Gge 3 Joseph Court Amhurst Park", AssetType = "LettableNonDwelling", PostCode = "N16 5AJ", UPRN = ""},
            new AddressStub{ FirstLine = "Gge 24 Stanway Court Geffrye Estate", AssetType = "LettableNonDwelling", PostCode = "N1 6RY", UPRN = ""},
            new AddressStub{ FirstLine = "Gge 106 Lincoln Court Bethune Road", AssetType = "LettableNonDwelling", PostCode = "N16 5EA", UPRN = ""},
            new AddressStub{ FirstLine = "Gge 53 Lincoln Court 1-66 Bethune Road", AssetType = "LettableNonDwelling", PostCode = "N16 5EB", UPRN = ""},
            new AddressStub{ FirstLine = "Gge 122 Lincoln Court Bethune Road", AssetType = "LettableNonDwelling", PostCode = "N16 5DZ", UPRN = ""},
            new AddressStub{ FirstLine = "172 Lincoln Court Bethune Road", AssetType = "Dwelling", PostCode = "N16 5DZ", UPRN = ""},
            new AddressStub{ FirstLine = "Gge 150 Lincoln Court Bethune Road Hackney LONDON ", AssetType = "LettableNonDwelling", PostCode = "N16 5EA", UPRN = ""},
            new AddressStub{ FirstLine = "150 Lincoln Court Bethune Road", AssetType = "Dwelling", PostCode = "N16 5DZ", UPRN = ""},
            new AddressStub{ FirstLine = "25 Vanbrugh House Loddiges Road", AssetType = "Dwelling", PostCode = "E9 7NP", UPRN = "100023023794"},
            new AddressStub{ FirstLine = "22 Loddiges House Loddiges Road", AssetType = "Dwelling", PostCode = "E9 7PJ", UPRN = "100023023850"},
            new AddressStub{ FirstLine = "2 Loddiges House Loddiges Road", AssetType = "Dwelling", PostCode = "E9 7PJ", UPRN = "100023023847"},
            new AddressStub{ FirstLine = "2 Sloane House Loddiges Road", AssetType = "Dwelling", PostCode = "E9 7NR", UPRN = "100023023822"},
            new AddressStub{ FirstLine = "21 Vanbrugh House Loddiges Road", AssetType = "Dwelling", PostCode = "E9 7NP", UPRN = "100023023790"},
            new AddressStub{ FirstLine = "21 Loddiges House Loddiges Road", AssetType = "Dwelling", PostCode = "E9 7PJ", UPRN = "100023023849"},
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
                asset.AssetAddress.AddressLine1 = value.FirstLine;
                asset.AssetType = value.AssetType;
                asset.AssetAddress.PostCode = value.PostCode;
                asset.AssetAddress.Uprn = value.UPRN;

                listOfAssets.Add(asset);
            }

            return listOfAssets;
        }
    }

    public class AddressStub
    {
        public string FirstLine { get; set; }
        public string AssetType { get; set; }
        public string PostCode { get; set; }
        public string UPRN { get; set; }
    }
}
