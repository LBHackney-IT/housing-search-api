using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Factories;
using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using Hackney.Shared.Processes.Domain;
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

        private static QueryablePatchAssignment _patchAssignment = _fixture.Create<QueryablePatchAssignment>();
        public static QueryableProcess[] Processes =
        {
            QueryableProcess.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "tenure", _fixture.CreateMany<QueryableRelatedEntity>().ToList(), ProcessName.soletojoint, _patchAssignment , SharedStates.DocumentsAppointmentRescheduled),
            QueryableProcess.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "person", _fixture.CreateMany<QueryableRelatedEntity>().ToList(), ProcessName.changeofname, _patchAssignment, SharedStates.DocumentChecksPassed),
            QueryableProcess.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "asset", _fixture.CreateMany<QueryableRelatedEntity>().ToList(), ProcessName.soletojoint, _patchAssignment,  SharedStates.ProcessCancelled),
            QueryableProcess.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "person", _fixture.CreateMany<QueryableRelatedEntity>().ToList(), ProcessName.changeofname, _patchAssignment, SharedStates.ProcessClosed),
            QueryableProcess.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "tenure", _fixture.CreateMany<QueryableRelatedEntity>().ToList(), ProcessName.soletojoint, _patchAssignment, SharedStates.ProcessCompleted),
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

                var awaitable = ElasticSearchClient.IndexManyAsync(Processes, INDEX).ConfigureAwait(true);

                while (!awaitable.GetAwaiter().IsCompleted)
                {

                }

                Thread.Sleep(5000);
            }
        }
    }
}
