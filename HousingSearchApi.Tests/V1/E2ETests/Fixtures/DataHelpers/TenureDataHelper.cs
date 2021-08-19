//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using AutoFixture;
//using Elasticsearch.Net;
//using HousingSearchApi.V1.Gateways.Models;
//using Nest;

//namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures.DataHelpers
//{
//    public static class TenureDataHelper
//    {
//        private const string INDEX = "persons";
//        public static string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

//        public static void GivenAPersonIndexExists(IElasticClient elasticClient)
//        {
//            if (!elasticClient.Indices.Exists(Indices.Index(INDEX)).Exists)
//            {
//                var personSettingsDoc = File.ReadAllTextAsync("./data/elasticsearch/tenureIndex.json").Result;
//                elasticClient.LowLevel.Indices.CreateAsync<BytesResponse>(INDEX, personSettingsDoc)
//                    .ConfigureAwait(true);

//                var persons = CreateTenureData();
//                elasticClient.IndexManyAsync(persons, INDEX);

//                var timeout = DateTime.UtcNow.AddSeconds(10); // 10 second timeout to make sure all the data is there.

//                while (DateTime.UtcNow < timeout)
//                {
//                    var count = elasticClient.Cluster.Stats().Indices.Documents.Count;
//                    if (count >= persons.Count)
//                        break;

//                    Thread.Sleep(200);
//                }
//            }
//        }

//        private static List<QueryableTenure> CreateTenureData()
//        {
//            var listOfTenures = new List<QueryableTenure>();
//            var random = new Random();
//            var fixture = new Fixture();

//            // Make sure there are 10 of each surname first in case there are tests that depend on some being there.
//            foreach (var name in Alphabet)
//            {
//                var tenures = fixture.Build<QueryableTenure>()
//                    .CreateMany(10);

//                listOfTenures.AddRange(tenures);
//            }

//            return listOfTenures;
//        }
//    }
//}
