using AutoFixture;
using HousingSearchApi.V1.Gateways.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Elasticsearch.Net;
using QueryableTenure = HousingSearchApi.V1.Gateways.Models.QueryableTenure;

namespace HousingSearchApi.Tests.V1.Helper
{
    public static class TestDataHelper
    {
        public static readonly string PersonIndex = "persons";
        public static readonly string TenureIndex = "tenures";
        public static string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

        public static void InsertDataInEs(IElasticClient elasticClient)
        {
            elasticClient.Indices.Delete(PersonIndex);

            //elasticClient.Indices.Create(TenureIndex, s =>
            //    s.Map(x => x.AutoMap()
            //        .Properties(prop =>
            //            prop.Keyword(field => field.Name("surname"))
            //                .Keyword(field => field.Name("firstname")))));

            elasticClient.Indices.Delete(TenureIndex);

            //elasticClient.Indices.Create(PersonIndex, s =>
            //    s.Map(x => x.AutoMap()
            //        .Properties(prop =>
            //            prop.Keyword(field => field.Name("surname"))
            //                .Keyword(field => field.Name("firstname")))));

            var personSettingsDoc = File.ReadAllTextAsync("./../../../../data/elasticsearch/personIndex.json").Result;
            elasticClient.LowLevel.Indices.CreateAsync<BytesResponse>(PersonIndex, personSettingsDoc).ConfigureAwait(true);
            var tenureSettingsDoc = File.ReadAllTextAsync("./../../../../data/elasticsearch/tenureIndex.json").Result;
            elasticClient.LowLevel.Indices
                .CreateAsync<BytesResponse>(TenureIndex, tenureSettingsDoc).ConfigureAwait(true);

            var persons = CreateQueryablePerson();
            var tenures = CreateQueryableTenure();

            elasticClient.IndexManyAsync(persons, PersonIndex);
            elasticClient.IndexManyAsync(tenures, TenureIndex);

            var timeout = DateTime.UtcNow.AddSeconds(10); // 10 second timeout to make sure all the data is there.

            while (DateTime.UtcNow < timeout)
            {
                var count = elasticClient.Cluster.Stats().Indices.Documents.Count;
                if (count >= persons.Count)
                    break;

                Thread.Sleep(200);
            }
        }

        private static List<QueryablePerson> CreateQueryablePerson()
        {
            var listOfPersons = new List<QueryablePerson>();
            var random = new Random();
            var fixture = new Fixture();

            // Make sure there are 10 of each surname first in case there are tests that depend on some being there.
            foreach (var name in Alphabet)
            {
                var persons = fixture.Build<QueryablePerson>()
                                    .With(x => x.Firstname, "Some first name")
                                    .With(x => x.Surname, name)
                                    .CreateMany(10);

                listOfPersons.AddRange(persons);
            }

            // Add loads more at random
            for (int i = 0; i < 900; i++)
            {
                var person = fixture.Create<QueryablePerson>();

                var firstName = Alphabet[random.Next(0, Alphabet.Length)];
                person.Firstname = firstName;

                var lastName = Alphabet[random.Next(0, Alphabet.Length)];
                person.Surname = lastName;

                listOfPersons.Add(person);
            }

            var lastPerson = fixture.Create<QueryablePerson>();
            lastPerson.Firstname = Alphabet.Last();
            lastPerson.Surname = Alphabet.Last();

            var firstPerson = fixture.Create<QueryablePerson>();
            firstPerson.Firstname = Alphabet.First();
            firstPerson.Surname = Alphabet.First();

            listOfPersons.Add(lastPerson);
            listOfPersons.Add(firstPerson);

            return listOfPersons;
        }

        private static List<QueryableTenure> CreateQueryableTenure()
        {
            var listOfTenures = new List<QueryableTenure>();
            var random = new Random();
            var fixture = new Fixture();

            // Make sure there are 10 of each surname first in case there are tests that depend on some being there.
            foreach (var name in Alphabet)
            {
                var tenures = fixture.Build<QueryableTenure>()
                    .CreateMany(10);

                listOfTenures.AddRange(tenures);
            }

            return listOfTenures;
        }
    }
}
