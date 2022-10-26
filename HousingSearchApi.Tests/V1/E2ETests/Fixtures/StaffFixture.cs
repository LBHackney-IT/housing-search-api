using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Factories;
using Hackney.Shared.HousingSearch.Gateways.Models.Staffs;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Staff = Hackney.Shared.HousingSearch.Domain.Staff.Staff;


namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class StaffFixture : BaseFixture
    {
        private static Fixture _fixture = new Fixture();
        private const string INDEX = "staff";

        public static Staff[] Staffs =
        {
            Staff.Create("firstName1", "lastName1", "firstName1.lastName1@test.com", Guid.NewGuid(), Guid.NewGuid()),
            Staff.Create("firstName2", "lastName2", "firstName2.lastName2@test.com", Guid.NewGuid(), Guid.NewGuid()),
            Staff.Create("firstName3", "lastName3", "firstName3.lastName3@test.com", Guid.NewGuid(), Guid.NewGuid()),
            Staff.Create("firstName4", "lastName4", "firstName4.lastName4@test.com", Guid.NewGuid(), Guid.NewGuid()),
            Staff.Create("firstName5", "lastName5", "firstName5.lastName5@test.com", Guid.NewGuid(), Guid.NewGuid())
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

                var staff = Staffs.Select(x => x.ToDatabase());
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
    }
}
