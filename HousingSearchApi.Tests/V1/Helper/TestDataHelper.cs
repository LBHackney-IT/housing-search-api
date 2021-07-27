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
            elasticClient?.Indices.Delete(Index);

            elasticClient?.Indices.Create(Index, s =>
                s.Map(x => x.AutoMap()
                    .Properties(prop =>
                        prop.Keyword(field => field.Name("surname"))
                            .Keyword(field => field.Name("firstname")))));

            var persons = CreateQueryablePerson();
            elasticClient.IndexManyAsync(persons, Index);

            Thread.Sleep(500);

            return persons;
        }

        private static List<QueryablePerson> CreateQueryablePerson()
        {
            var listOfPersons = new List<QueryablePerson>();
            var random = new Random();
            var fixture = new Fixture();

            for (int i = 0; i < 1000; i++)
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
