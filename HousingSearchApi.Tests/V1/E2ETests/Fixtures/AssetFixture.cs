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
            new AddressStub{ FirstLine = "59 Buckland Court St Johns Estate", AssetType = "Dwelling", PostCode = "N1 5EP", UPRN = "10008234650",
                AssetStatus = "Reserved", NoOfBedSpaces = 2, NoOfCots = 1, HasStairs = true, PrivateBathroom = true, PrivateKitchen = true, StepFree = true },
            new AddressStub{ FirstLine = "54 Buckland Court St Johns Estate", AssetType = "Estate", PostCode = "N1 5EP", UPRN = "10008234655",
                AssetStatus = "Archived", NoOfBedSpaces = 3, NoOfCots = 0, HasStairs = false, PrivateBathroom = true, PrivateKitchen = true, StepFree = false },
            new AddressStub{ FirstLine = "65 Buckland Court St Johns Estate", AssetType = "TerracedBlock", PostCode = "N1 5EP", UPRN = "10008234605",
                AssetStatus = "In council use", NoOfBedSpaces = 4, NoOfCots = 2, HasStairs = true, PrivateBathroom = false, PrivateKitchen = false, StepFree = true },
            new AddressStub{ FirstLine = "45 Buckland Court St Johns Estate", AssetType = "HighRiseBlock", PostCode = "N1 5EP", UPRN = "10008234650",
                AssetStatus = "Occupied", NoOfBedSpaces = 5, NoOfCots = 1, HasStairs = false, PrivateBathroom = true, PrivateKitchen = false, StepFree = true },
            new AddressStub{ FirstLine = "Gge 45 Buckland Court St Johns Estate", AssetType = "Block", PostCode = "N1 5EP", UPRN = "10008234650",
                AssetStatus = "Vacant", NoOfBedSpaces = 1, NoOfCots = 0, HasStairs = true, PrivateBathroom = false, PrivateKitchen = true, StepFree = false },
            new AddressStub{ FirstLine = "Gge 52 Buckland Court St Johns Estate", AssetType = "Dwelling", PostCode = "N1 5EP", UPRN = "10008234650",
                AssetStatus = "Void", NoOfBedSpaces = 2, NoOfCots = 0, HasStairs = false, PrivateBathroom = false, PrivateKitchen = true, StepFree = true },
            new AddressStub{ FirstLine = "Gge 51 Buckland Court St Johns Estate", AssetType = "ThirdAsset", PostCode = "N1 5EP", UPRN = "10008234650"},
            new AddressStub{ FirstLine = "5 Buckland Court St Johns Estate", AssetType = "FirstAsset", PostCode = "N1 6TY", UPRN = "10008235183"},
            new AddressStub{ FirstLine = "Gge 15 Buckland Court St Johns Estate", AssetType = "SecondAsset", PostCode = "N1 5EP", UPRN = "10008234650"},
            new AddressStub{ FirstLine = "Gge 53 Buckland Court St Johns Estate", AssetType = "ThirdAsset", PostCode = "N1 5EP", UPRN = "10008234650"},
            new AddressStub{ FirstLine = "Gge 25 Buckland Court St Johns Estate", AssetType = "SecondAsset", PostCode = "N1 5EP", UPRN = "10008234650"}
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
                asset.AssetStatus = value.AssetStatus;
                asset.AssetCharacteristics.NumberOfBedSpaces = value.NoOfBedSpaces;
                asset.AssetCharacteristics.NumberOfCots = value.NoOfCots;
                asset.AssetCharacteristics.HasStairs = value.HasStairs;
                asset.AssetCharacteristics.HasPrivateBathroom = value.PrivateBathroom;
                asset.AssetCharacteristics.HasPrivateKitchen = value.PrivateKitchen;
                asset.AssetCharacteristics.IsStepFree = value.StepFree;
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
        public string AssetStatus { get; set; }
        public int NoOfBedSpaces { get; set; }
        public int NoOfCots { get; set; }
        public bool HasStairs { get; set; }
        public bool PrivateBathroom { get; set; }
        public bool PrivateKitchen { get; set; }
        public bool StepFree { get; set; }
    }
}
