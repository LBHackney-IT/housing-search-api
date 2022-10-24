using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Domain.Process;
using Hackney.Shared.HousingSearch.Factories;
using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using Hackney.Shared.Processes.Domain.Constants;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Process = Hackney.Shared.HousingSearch.Domain.Process.Process;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class ProcessFixture : BaseFixture
    {
        public const string INDEX = "processes";
        private static readonly Fixture _fixture = new Fixture();

        public static PatchAssignment PatchAssignment = _fixture.Create<PatchAssignment>();

        private static DateTime _processStartedAt = DateTime.UtcNow;

        private static DateTime _stateStartedAt = DateTime.UtcNow;

        public static Process[] Processes =
        {
            Process.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "tenure", _fixture.CreateMany<RelatedEntity>().ToList(), "soletojoint", PatchAssignment , SharedStates.DocumentsAppointmentRescheduled, _processStartedAt, _stateStartedAt),
            Process.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "person", _fixture.CreateMany<RelatedEntity>().ToList(), "changeofname", PatchAssignment, SharedStates.DocumentChecksPassed,  _processStartedAt, _stateStartedAt),
            Process.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "asset", _fixture.CreateMany<RelatedEntity>().ToList(), "soletojoint", PatchAssignment,  SharedStates.ProcessCancelled,  _processStartedAt, _stateStartedAt),
            Process.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "person", _fixture.CreateMany<RelatedEntity>().ToList(), "changeofname", PatchAssignment, SharedStates.ProcessClosed, _processStartedAt, _stateStartedAt),
            Process.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "tenure", _fixture.CreateMany<RelatedEntity>().ToList(), "soletojoint", PatchAssignment, SharedStates.ProcessCompleted, _processStartedAt, _stateStartedAt),
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
                var awaitable = ElasticSearchClient.IndexManyAsync(processes, INDEX).ConfigureAwait(true);

                while (!awaitable.GetAwaiter().IsCompleted)
                {

                }

                Thread.Sleep(5000);
            }
        }
    }
}
