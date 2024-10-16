using Nest;
using System;
using System.IO;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Xunit;

namespace HousingSearchApi.Tests.V2.E2ETests.Fixtures;

public class ElasticsearchFixture : IAsyncLifetime
{
    public IElasticClient Client { get; private set; }

    public ElasticsearchFixture()
    {
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("assets");

        Client = new ElasticClient(settings);
    }

    public async Task InitializeAsync()
    {
        string jsonFilePath = "V2/E2ETests/Fixtures/assets.json";
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"The file {jsonFilePath} could not be found in directory {Directory.GetCurrentDirectory()}");
        }

        string jsonContent = File.ReadAllText(jsonFilePath);

        var bulkResponse = await Client.LowLevel.BulkAsync<StringResponse>(PostData.String(jsonContent));

        if (!bulkResponse.Success)
        {
            throw new Exception("Bulk insert failed: " + bulkResponse.DebugInformation);
        }
    }

    public Task DisposeAsync()
    {
        // Clean up if necessary, e.g., deleting the index
        return Task.CompletedTask;
    }
}
