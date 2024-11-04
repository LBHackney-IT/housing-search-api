using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Domain.Process;
using Hackney.Shared.HousingSearch.Factories;
using Hackney.Shared.Processes.Domain.Constants;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Process = Hackney.Shared.HousingSearch.Domain.Process.Process;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class ProcessFixture : BaseFixture
    {
        public const string INDEX = "processes";
        private const int NumberOfGeneratedProcesses = 5;
        private static readonly Fixture _fixture = new Fixture();

        public static PatchAssignment PatchAssignment = _fixture.Create<PatchAssignment>();

        private static DateTime _processStartedAt = DateTime.UtcNow;

        private static DateTime _stateStartedAt = DateTime.UtcNow;

        internal static List<List<RelatedEntity>> _relatedEntities = GenerateRelatedEntities(NumberOfGeneratedProcesses);

        public static Process[] Processes =
        {
            Process.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "tenure", _relatedEntities.ElementAtOrDefault(0), "soletojoint", PatchAssignment, SharedStates.DocumentsAppointmentRescheduled, _processStartedAt, _stateStartedAt),
            Process.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "person", _relatedEntities.ElementAtOrDefault(1), "changeofname", PatchAssignment, SharedStates.DocumentChecksPassed,  _processStartedAt, _stateStartedAt),
            Process.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "asset", _relatedEntities.ElementAtOrDefault(2), "soletojoint", PatchAssignment,  SharedStates.ProcessCancelled,  _processStartedAt, _stateStartedAt),
            Process.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "person", _relatedEntities.ElementAtOrDefault(3), "changeofname", PatchAssignment, SharedStates.ProcessClosed, _processStartedAt, _stateStartedAt),
            Process.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "tenure", _relatedEntities.ElementAtOrDefault(4), "soletojoint", PatchAssignment, SharedStates.ProcessCompleted, _processStartedAt, _stateStartedAt),
        };


        public ProcessFixture(IElasticClient elasticClient, HttpClient httpClient) : base(elasticClient, httpClient)
        {
            WaitForESInstance();
        }

        public void GivenAnProcessIndexExists()
        {
            ElasticSearchClient.Indices.Delete(INDEX);

            if (!ElasticSearchClient.Indices.Exists(Indices.Index(INDEX)).Exists)
            {
                var processSettingsDoc = File.ReadAllTextAsync("./data/elasticsearch/processesIndex.json").Result;
                ElasticSearchClient.LowLevel.Indices.CreateAsync<BytesResponse>(INDEX, processSettingsDoc)
                                                    .ConfigureAwait(true);

                var processes = Processes.Select(x => x.ToDatabase());

                ElasticSearchClient.IndexMany(processes, INDEX);
                do
                    Thread.Sleep(100);
                while (!ElasticSearchClient.Indices.Refresh(Indices.Index(INDEX)).IsValid);
            }
        }

        private static List<List<RelatedEntity>> GenerateRelatedEntities(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var allRelatedEntities = new List<List<RelatedEntity>>();
            for (int i = 0; i < count; i++)
            {
                var relatedEntityPerson = new RelatedEntity()
                {
                    Id = "Ide1f4712b-b0af-4438-8037-edbde301c77c",
                    Description = $"test person {i}",
                    SubType = "Ide1f4712b-b0af-4438-8037-edbde301c77c",
                    TargetType = "person"
                };
                var relatedEntityTenure = new RelatedEntity()
                {
                    Id = "Ide1f4712b-b0af-4438-8037-edbde301c77c",
                    Description = $"test tenure {i}",
                    SubType = "Ide1f4712b-b0af-4438-8037-edbde301c77c",
                    TargetType = "tenure"
                };

                var entitiesCreated = new List<RelatedEntity>() { relatedEntityPerson, relatedEntityTenure };
                allRelatedEntities.Add(entitiesCreated);
            }

            return allRelatedEntities;
        }
    }
}
