using AutoFixture;
using HousingSearchApi.V1.Gateways.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HousingSearchApi.Tests.V1.Helper
{
    public static class TestDataHelper
    {
        public static readonly string Index = "persons";
        public static string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

        public static List<QueryablePerson> InsertPersonsInEs(IElasticClient elasticClient)
        {
            elasticClient.Indices.Delete(Index);

            elasticClient.Indices.Create(Index, s =>
                s.Map(x => x.AutoMap()
                    .Properties(prop =>
                        prop.Keyword(field => field.Name("surname"))
                            .Keyword(field => field.Name("firstname")))));

            var persons = CreateQueryablePerson();
            elasticClient.IndexManyAsync(persons, Index);

            var timeout = DateTime.UtcNow.AddSeconds(10); // 10 second timeout to make sure all the data is there.
            while (DateTime.UtcNow < timeout)
            {
                var count = elasticClient.Cluster.Stats().Indices.Documents.Count;
                if (count >= persons.Count)
                    break;

                Thread.Sleep(200);
            }

            return persons;
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
    }
}
