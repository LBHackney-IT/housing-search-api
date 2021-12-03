using System;
using System.Net.Http;
using System.Threading;
using Nest;

namespace HousingSearchApi.Tests.V1.E2ETests.Fixtures
{
    public class BaseFixture
    {
        protected string INDEX;
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
    }
}
