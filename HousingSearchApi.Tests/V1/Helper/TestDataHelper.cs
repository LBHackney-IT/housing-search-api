using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.Tests.V1.Helper
{
    public static class TestDataHelper
    {
        public static string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

        public static void InsertPersonInEs(IElasticClient elasticClient, string key = null,
            QueryablePerson addressConfig = null)
        {
            elasticClient.Indices.Delete("persons");

            elasticClient.Indices.Create("persons", s =>
                s.Map(x => x.AutoMap()
                    .Properties(prop =>
                        prop.Keyword(field => field.Name("surname"))
                            .Keyword(field => field.Name("firstname")))));

            elasticClient.IndexManyAsync(CreateQueryablePerson(), "persons");
        }

        private static List<QueryablePerson> CreateQueryablePerson()
        {
            var listOfPersons = new List<QueryablePerson>();
            var random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                var fixture = new Fixture();
                var person = fixture.Create<QueryablePerson>();

                var firstName = Alphabet[random.Next(0, Alphabet.Length)];
                person.Firstname = firstName;

                var lastName = Alphabet[random.Next(0, Alphabet.Length)];
                person.Surname = lastName;

                listOfPersons.Add(person);
            }

            var firstAndLastFixture = new Fixture();

            var lastPerson = firstAndLastFixture.Create<QueryablePerson>();
            lastPerson.Firstname = Alphabet.Last();
            lastPerson.Surname = Alphabet.Last();

            var firstPerson = firstAndLastFixture.Create<QueryablePerson>();
            firstPerson.Firstname = Alphabet.First();
            firstPerson.Surname = Alphabet.First();

            listOfPersons.Add(lastPerson);
            listOfPersons.Add(firstPerson);

            return listOfPersons;
        }
    }
}
