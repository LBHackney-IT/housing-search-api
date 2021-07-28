using HousingSearchApi.V1.Gateways.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class ElasticSearchFixture : IDisposable
    {
        public IElasticClient ElasticSearchClient => _factory?.ElasticSearchClient;
        public HttpClient Client { get; private set; }

        private readonly MockWebApplicationFactory<Startup> _factory;
        private static string _esAddress;

        public List<QueryablePerson> Persons { get; private set; }
        private readonly List<Action> _cleanup = new List<Action>();

        public ElasticSearchFixture()
        {
            EnsureEnvVarConfigured("ELASTICSEARCH_DOMAIN_URL", "http://localhost:9200");

            _factory = new MockWebApplicationFactory<Startup>();
            Client = _factory.CreateClient();

            WaitForESInstance(ElasticSearchClient);
            Persons = TestDataHelper.InsertPersonsInEs(ElasticSearchClient);

            _cleanup.Add(() =>
            {
                ElasticSearchClient.DeleteManyAsync(Persons, TestDataHelper.Index)
                                   .GetAwaiter().GetResult();
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                foreach (var action in _cleanup)
                    action();

                if (null != _factory)
                    _factory.Dispose();
                _disposed = true;
            }
        }

        private void WaitForESInstance(IElasticClient elasticSearchClient)
        {
            Exception ex = null;
            var timeout = DateTime.UtcNow.AddSeconds(10); // 5 second timeout (make configurable?)
            while (DateTime.UtcNow < timeout)
            {
                try
                {
                    var pingResponse = elasticSearchClient.Ping();
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
                throw new Exception($"Could not connect to ES instance on {_esAddress}", ex);
            }
        }

        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            _esAddress = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrEmpty(_esAddress))
            {
                Environment.SetEnvironmentVariable(name, defaultValue);
                _esAddress = default;
            }
        }
    }
}
