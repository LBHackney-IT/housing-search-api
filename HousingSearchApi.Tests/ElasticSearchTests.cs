using System;
using System.IO;
using System.Threading.Tasks;
using Elasticsearch.Net;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using NUnit.Framework;

namespace HousingSearchApi.Tests
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

            if (client == null)
                return;

            await CreateIndeces(Constants.EsIndex, client).ConfigureAwait(true);
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

        private static async Task CreateIndeces(string name, IElasticClient client)
        {
            var personSettingsDoc = await File.ReadAllTextAsync("./../../../../data/elasticsearch/personIndex.json")
                .ConfigureAwait(true);

            await client.LowLevel.Indices.CreateAsync<BytesResponse>(name, personSettingsDoc)
                .ConfigureAwait(true);

            var tenureSettingsDoc = await File.ReadAllTextAsync("./../../../../data/elasticsearch/tenureIndex.json")
                .ConfigureAwait(true);

            await client.LowLevel.Indices.CreateAsync<BytesResponse>(name, tenureSettingsDoc)
                .ConfigureAwait(true);
        }

        public static void DeleteAddressesIndex(ElasticClient client)
        {
            if (client == null)
                return;

            if (client.Indices.Exists(Constants.EsIndex).Exists)
            {
                client.Indices.Delete(Constants.EsIndex);
            }
        }
    }
}
