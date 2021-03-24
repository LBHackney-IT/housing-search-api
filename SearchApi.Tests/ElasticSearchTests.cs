using System;
using System.IO;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using NUnit.Framework;
using SearchApi.V1.Infrastructure;

namespace SearchApi.Tests
{
    [TestFixture]
    public class ElasticsearchTests
    {
        protected ElasticClient ElasticsearchClient { get; private set; }

        [OneTimeSetUp]
        public void BeforeAllElasticsearchTests()
        {
            ElasticsearchClient = SetupElasticsearchConnection();
        }

        [SetUp]
        public async Task SetupElasticsearchClient()
        {
            await BeforeAnyElasticsearchTest(ElasticsearchClient).ConfigureAwait(true);
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
            DeleteAddressesIndex(ElasticsearchClient);
        }

        public static async Task BeforeAnyElasticsearchTest(ElasticClient client)
        {
            DeleteAddressesIndex(client);

            //TODO: Index?
            await CreateIndex(Constants.EsIndex, client).ConfigureAwait(true);
        }
        public static ElasticClient SetupElasticsearchConnection()
        {
            var esDomainUri = Environment.GetEnvironmentVariable("ELASTICSEARCH_DOMAIN_URL")
                              ?? "http://localhost:9202";
            using var pool = new SingleNodeConnectionPool(new Uri(esDomainUri));
            using var settings = new ConnectionSettings(pool).PrettyJson()
                .DisableDirectStreaming()
                .SniffOnStartup(false)
                .ThrowExceptions();

            return new ElasticClient(settings);
        }

        private static async Task CreateIndex(string name, IElasticClient client)
        {
            var settingsDoc = await File.ReadAllTextAsync("./../../../../data/elasticsearch/index.json")
                .ConfigureAwait(true);

            await client.LowLevel.Indices.CreateAsync<BytesResponse>(name, settingsDoc)
                .ConfigureAwait(true);
        }

        public static void DeleteAddressesIndex(ElasticClient client)
        {
            if (client.Indices.Exists(Constants.EsIndex).Exists)
            {
                client.Indices.Delete(Constants.EsIndex);
            }
        }
    }
}
