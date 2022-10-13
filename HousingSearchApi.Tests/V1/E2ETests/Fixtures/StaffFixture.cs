using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Gateways.Models.Staffs;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class StaffFixture : BaseFixture
    {
        private const string INDEX = "staff";

        public static StaffStub[] Staffs =
        {
            new StaffStub{FirstName = "firstName1", LastName = "lastName1", EmailAddress = "firstname1.lastname1@test.com", PatchId = Guid.NewGuid(), AreaId = Guid.NewGuid()},
            new StaffStub{FirstName = "firstName2", LastName = "lastName2", EmailAddress = "firstname2.lastname2@test.com", PatchId = Guid.NewGuid(), AreaId = Guid.NewGuid()},
            new StaffStub{FirstName = "firstName3", LastName = "lastName3", EmailAddress = "firstname3.lastname3@test.com", PatchId = Guid.NewGuid(), AreaId = Guid.NewGuid()},
            new StaffStub{FirstName = "firstName4", LastName = "lastName4", EmailAddress = "firstname4.lastname4@test.com", PatchId = Guid.NewGuid(), AreaId = Guid.NewGuid()},
            new StaffStub{FirstName = "firstName5", LastName = "lastName5", EmailAddress = "firstname5.lastname5@test.com", PatchId = Guid.NewGuid(), AreaId = Guid.NewGuid()}
        };

        public StaffFixture(IElasticClient elasticClient, HttpClient httpClient) : base(elasticClient, httpClient)
        {
            WaitForESInstance();
        }

        public void GivenAnStaffIndexExists()
        {
            ElasticSearchClient.Indices.Delete(INDEX);

            if (!ElasticSearchClient.Indices.Exists(Indices.Index(INDEX)).Exists)
            {
                var staffSettingsDoc = File.ReadAllTextAsync("./data/elasticsearch/staffIndex.json").Result;
                ElasticSearchClient.LowLevel.Indices.CreateAsync<BytesResponse>(INDEX, staffSettingsDoc)
                    .ConfigureAwait(true);

                var staff = CreateStaffData();
                var awaitable = ElasticSearchClient.IndexManyAsync(staff, INDEX).ConfigureAwait(true);

                while (!awaitable.GetAwaiter().IsCompleted)
                {

                }

                Thread.Sleep(5000);
            }
        }

        public void GivenThereExistStaffsWithSimilarEmailAddress(string emailAddress)
        {
            var fixture = new Fixture();
            var listOfStaffs = new List<QueryableStaff>();

            var specificStaff = fixture.Create<QueryableStaff>();
            specificStaff.EmailAddress = emailAddress;
            listOfStaffs.Add(specificStaff);

            var specificStaff2 = fixture.Create<QueryableStaff>();
            specificStaff2.EmailAddress = "Something.new@test.com";
            listOfStaffs.Add(specificStaff2);

            var specificStaff3 = fixture.Create<QueryableStaff>();
            specificStaff3.EmailAddress = emailAddress + emailAddress;
            listOfStaffs.Add(specificStaff3);

            var awaitable = ElasticSearchClient.IndexManyAsync(listOfStaffs, INDEX).ConfigureAwait(true);

            while (!awaitable.GetAwaiter().IsCompleted) { }

            Thread.Sleep(5000);
        }

        private List<QueryableStaff> CreateStaffData()
        {
            var listOfStaffs = new List<QueryableStaff>();
            var fixture = new Fixture();
            var random = new Random();

            foreach (var value in Staffs)
            {
                var staff = fixture.Create<QueryableStaff>();
                staff.FirstName = value.FirstName;
                staff.LastName = value.LastName;
                staff.EmailAddress = value.EmailAddress;
                staff.PatchId = value.PatchId;
                staff.AreaId = value.AreaId;
                listOfStaffs.Add(staff);
            }

            return listOfStaffs;
        }
    }

    public class StaffStub
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Guid? PatchId { get; set; }
        public Guid? AreaId { get; set; }

    }
}
