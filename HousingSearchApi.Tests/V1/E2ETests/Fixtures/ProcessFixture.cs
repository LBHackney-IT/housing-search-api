using AutoFixture;
using Elasticsearch.Net;
using Hackney.Shared.HousingSearch.Domain.Process;
using Hackney.Shared.HousingSearch.Gateways.Models.Processes;
using Hackney.Shared.Processes.Domain;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using RelatedEntity = Hackney.Shared.HousingSearch.Domain.Process.RelatedEntity;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class ProcessFixture : BaseFixture
    {
        public const string INDEX = "processes";
        private static readonly Fixture _fixture = new Fixture();

        public static ProcessStub[] Processes =
        {
            new ProcessStub{ Id = Guid.NewGuid().ToString(), TargetId = Guid.NewGuid().ToString(), TargetType = TargetType.tenure, ProcessName = ProcessName.soletojoint, State = "ProcessStarted", PatchAssignment = _fixture.Create<PatchAssignment>(), RelatedEntities = _fixture.CreateMany<RelatedEntity>().ToList()},
            new ProcessStub{ Id = Guid.NewGuid().ToString(), TargetId = Guid.NewGuid().ToString(), TargetType = TargetType.person, ProcessName = ProcessName.changeofname, State = "ProcessUpdated", PatchAssignment = _fixture.Create<PatchAssignment>(), RelatedEntities = _fixture.CreateMany<RelatedEntity>().ToList()},
            new ProcessStub{ Id = Guid.NewGuid().ToString(), TargetId = Guid.NewGuid().ToString(), TargetType = TargetType.asset, ProcessName = ProcessName.soletojoint, State = "ProcessClosed", PatchAssignment = _fixture.Create<PatchAssignment>(), RelatedEntities = _fixture.CreateMany<RelatedEntity>().ToList()},
            new ProcessStub{ Id = Guid.NewGuid().ToString(), TargetId = Guid.NewGuid().ToString(), TargetType = TargetType.person, ProcessName = ProcessName.changeofname, State = "ProcessCancelled", PatchAssignment = _fixture.Create<PatchAssignment>(), RelatedEntities = _fixture.CreateMany<RelatedEntity>().ToList()},
            new ProcessStub{ Id = Guid.NewGuid().ToString(), TargetId = Guid.NewGuid().ToString(), TargetType = TargetType.tenure, ProcessName = ProcessName.soletojoint, State = "ProcessCompleted", PatchAssignment = _fixture.Create<PatchAssignment>(), RelatedEntities = _fixture.CreateMany<RelatedEntity>().ToList()},
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

                var processes = CreateProcessData();
                var awaitable = ElasticSearchClient.IndexManyAsync(processes, INDEX).ConfigureAwait(true);

                while (!awaitable.GetAwaiter().IsCompleted)
                {

                }

                Thread.Sleep(5000);
            }
        }

        private List<QueryableProcess> CreateProcessData()
        {
            var listOfProcesses = new List<QueryableProcess>();

            foreach (var value in Processes)
            {
                var process = _fixture.Create<QueryableProcess>();
                process.Id = value.Id;
                process.TargetId = value.TargetId;
                process.TargetType = value.TargetType;
                process.ProcessName = value.ProcessName;
                process.State = value.State;
                process.PatchAssignment.PatchId = value.PatchAssignment.PatchId;
                process.PatchAssignment.PatchName = value.PatchAssignment.PatchName;
                process.PatchAssignment.ResponsibleName = value.PatchAssignment.ResponsibleName;
                process.PatchAssignment.ResponsibleEntityId = value.PatchAssignment.ResponsibleEntityId;
                process.RelatedEntities.FirstOrDefault().Id = value.RelatedEntities.FirstOrDefault().Id;
                process.RelatedEntities.FirstOrDefault().TargetType = value.RelatedEntities.FirstOrDefault().TargetType;
                process.RelatedEntities.FirstOrDefault().SubType = value.RelatedEntities.FirstOrDefault().SubType;
                process.RelatedEntities.FirstOrDefault().Description = value.RelatedEntities.FirstOrDefault().Description;
                listOfProcesses.Add(process);
            }

            return listOfProcesses;
        }
    }

    public class ProcessStub
    {
        public string Id { get; set; }
        public string TargetId { get; set; }

        public TargetType TargetType { get; set; }
        public ProcessName ProcessName { get; set; }
        public string State { get; set; }
        public PatchAssignment PatchAssignment { get; set; }
        public List<RelatedEntity> RelatedEntities { get; set; }
    }
}
