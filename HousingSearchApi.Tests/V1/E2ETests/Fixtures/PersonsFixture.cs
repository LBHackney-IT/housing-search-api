using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using HousingSearchApi.V1.Gateways.Models;
using Nest;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class PersonsFixture : BaseFixture
    {
        public List<QueryablePerson> Persons { get; private set; }
        private const string INDEX = "persons";
        public static string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

        public PersonsFixture(IElasticClient elasticClient, HttpClient httpClient) : base(elasticClient, httpClient)
        {
            WaitForESInstance();
        }

        public void GivenAPersonIndexExists()
        {
            ElasticSearchClient.Indices.Delete(Indices.Index(INDEX));

            if (!ElasticSearchClient.Indices.Exists(Indices.Index(INDEX)).Exists)
            {
                var personSettingsDoc = File.ReadAllTextAsync("./data/elasticsearch/personIndex.json").Result;
                ElasticSearchClient.LowLevel.Indices.CreateAsync<BytesResponse>(INDEX, personSettingsDoc)
                    .ConfigureAwait(true);

                var persons = CreatePersonData();
                var awaitable = ElasticSearchClient.IndexManyAsync(persons, INDEX).ConfigureAwait(true);

                while (!awaitable.GetAwaiter().IsCompleted)
                {

                }

                Thread.Sleep(5000);
            }
        }

        private List<QueryablePerson> CreatePersonData()
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

            // Add first and last name combo
            AddThreePersonsWithSimilarNames(fixture, listOfPersons);

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

        private void AddThreePersonsWithSimilarNames(Fixture fixture, List<QueryablePerson> listOfPersons)
        {
            var specificPerson = fixture.Create<QueryablePerson>();
            specificPerson.Firstname = "First";
            specificPerson.Surname = "Last";
            listOfPersons.Add(specificPerson);

            var specificPerson2 = fixture.Create<QueryablePerson>();
            specificPerson2.Firstname = "First";
            specificPerson2.Surname = "Something";
            listOfPersons.Add(specificPerson);

            var specificPerson3 = fixture.Create<QueryablePerson>();
            specificPerson3.Firstname = "Something";
            specificPerson3.Surname = "Last";
            listOfPersons.Add(specificPerson);
        }
    }
}
