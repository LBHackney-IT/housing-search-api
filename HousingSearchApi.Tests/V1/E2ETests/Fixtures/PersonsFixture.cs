using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Domain;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class PersonsFixture : BaseFixture
    {
        private const string INDEX = "persons";
        public static string[] Alphabet = { "aa", "bb", "cc", "dd", "ee", "vv", "ww", "xx", "yy", "zz" };

        public PersonsFixture(IElasticClient elasticClient, HttpClient httpClient) : base(elasticClient, httpClient)
        {
            WaitForESInstance();
        }

        public void GivenAPersonIndexExists()
        {
            // clear index
            ElasticSearchClient.Indices.Delete(Indices.Index(INDEX));
            var personSettingsDoc = File.ReadAllText("./data/elasticsearch/tenureIndex.json");
            ElasticSearchClient.LowLevel.Indices.Create<BytesResponse>(INDEX, personSettingsDoc);
            ElasticSearchClient.Indices.Refresh(Indices.Index(INDEX));

            // load data
            var persons = CreatePersonData();
            ElasticSearchClient.IndexMany(persons, INDEX);
            ElasticSearchClient.Indices.Refresh(Indices.Index(INDEX));

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

            var allPersonTypes = PersonType.Leaseholder.GetPersonTypes().Concat(PersonType.Tenant.GetPersonTypes()).ToList();

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

        public void GivenThereExistPersonsWithSimilarFirstAndLastNames(string firstName, string lastName)
        {
            var fixture = new Fixture();
            var listOfPersons = new List<QueryablePerson>();

            var specificPerson = fixture.Create<QueryablePerson>();
            specificPerson.Firstname = firstName;
            specificPerson.Surname = lastName;
            listOfPersons.Add(specificPerson);

            var specificPerson2 = fixture.Create<QueryablePerson>();
            specificPerson2.Firstname = firstName;
            specificPerson2.Surname = "Something";
            listOfPersons.Add(specificPerson2);

            var specificPerson3 = fixture.Create<QueryablePerson>();
            specificPerson3.Firstname = lastName;
            specificPerson3.Surname = "Last";
            listOfPersons.Add(specificPerson3);

            var specificPerson4 = fixture.Create<QueryablePerson>();
            specificPerson3.Firstname = firstName + firstName;
            specificPerson3.Surname = lastName + lastName;
            listOfPersons.Add(specificPerson4);

            ElasticSearchClient.IndexMany(listOfPersons, INDEX);
            ElasticSearchClient.Indices.Refresh(Indices.Index(INDEX));
        }

        public void GivenDifferentTypesOfTenureTypes(string firstName, string lastName, List<string> list)
        {
            var listOfPersons = new List<QueryablePerson>();

            foreach (var tenureType in list)
            {
                listOfPersons.Add(new QueryablePerson
                {
                    Firstname = firstName,
                    Surname = lastName,
                    Tenures = new List<QueryablePersonTenure>
                    {
                        new QueryablePersonTenure
                        {
                            Type = tenureType
                        }
                    }
                });
            }

            ElasticSearchClient.IndexMany(listOfPersons, INDEX);
            ElasticSearchClient.Indices.Refresh(Indices.Index(INDEX));
        }

        public void GivenThereExistPersonsWithDifferentPersonTypes(string firstName, string lastName, List<PersonType> personTypes)
        {
            var listOfPersons = new List<QueryablePerson>();

            foreach (var personType in personTypes)
            {
                listOfPersons.Add(new QueryablePerson
                {
                    Firstname = firstName,
                    Surname = lastName,
                    PersonTypes = new List<string> { personType.ToString() }
                });
            }

            ElasticSearchClient.IndexMany(listOfPersons, INDEX);
            ElasticSearchClient.Indices.Refresh(Indices.Index(INDEX));
        }

        public void GivenThereExistPersonsWithDifferentPersonTypes(string firstName, string lastName, List<PersonType> personTypes)
        {
            var listOfPersons = new List<QueryablePerson>();

            foreach (var personType in personTypes)
            {
                listOfPersons.Add(new QueryablePerson
                {
                    Firstname = firstName,
                    Surname = lastName,
                    PersonTypes = new List<string> { personType.ToString() }
                });
            }

            ElasticSearchClient.IndexMany(listOfPersons, INDEX);
            ElasticSearchClient.Indices.Refresh(Indices.Index(INDEX));
        }
    }
}
