using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using AutoFixture;
using Elasticsearch.Net;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
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

                persons.ToList().ForEach(p => p.Tenures.Add(new QueryablePersonTenure()
                {
                    Type = "Secure"
                }));

                listOfPersons.AddRange(persons);
            }

            var allPersonTypes = PersonType.Leaseholder.GetPersonTypes().Concat(PersonType.Rent.GetPersonTypes()).ToList();

            // Add loads more at random
            for (int i = 0; i < 900; i++)
            {
                var person = fixture.Create<QueryablePerson>();

                var firstName = Alphabet[random.Next(0, Alphabet.Length)];
                person.Firstname = firstName;

                var lastName = Alphabet[random.Next(0, Alphabet.Length)];
                person.Surname = lastName;

                person.Tenures.Add(new QueryablePersonTenure()
                {
                    Type = allPersonTypes[random.Next(0, allPersonTypes.Count - 1)]
                });

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
