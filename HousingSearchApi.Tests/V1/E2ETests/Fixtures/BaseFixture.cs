using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Amazon.DynamoDBv2;
using Hackney.Shared.HousingSearch.Domain.Process;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Nest;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class BaseFixture
    {
        protected IElasticClient ElasticSearchClient;
        public HttpClient HttpClient { get; set; }

        private string _elasticSearchAddress;

        public BaseFixture(IElasticClient elasticClient, HttpClient httpHttpClient)
        {
            ElasticSearchClient = elasticClient;
            HttpClient = httpHttpClient;
        }

        protected void WaitForESInstance()
        {
            EnsureEnvVarConfigured("ELASTICSEARCH_DOMAIN_URL", "http://localhost:9200");

            Exception ex = null;
            var timeout = DateTime.UtcNow.AddSeconds(20); // 10 second timeout to make sure the ES instance has started and is ready to use.
            while (DateTime.UtcNow < timeout)
            {
                try
                {
                    var pingResponse = ElasticSearchClient.Ping();
                    if (pingResponse.IsValid)
                        return;
                    else
                        ex = pingResponse.OriginalException;
                }
                catch (Exception e)
                {
                    ex = e;
                }

                Thread.Sleep(200);
            }

            if (ex != null)
            {
                throw new Exception($"Could not connect to ES instance on {_elasticSearchAddress}", ex);
            }
        }

        protected void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            _elasticSearchAddress = Environment.GetEnvironmentVariable(name);

            if (string.IsNullOrEmpty(_elasticSearchAddress))
            {
                Environment.SetEnvironmentVariable(name, defaultValue);
                _elasticSearchAddress = default;
            }
        }

        protected static List<List<RelatedEntity>> GenerateRelatedEntities(int count)
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
