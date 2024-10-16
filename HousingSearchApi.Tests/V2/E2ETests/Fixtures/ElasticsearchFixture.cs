using Nest;
using System;
using System.IO;
using System.Threading.Tasks;

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
        // This will run once before all tests in this test collection
        string jsonFilePath = "Path/To/Your/assets.json"; // Update the path accordingly
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"The file {jsonFilePath} could not be found.");
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
