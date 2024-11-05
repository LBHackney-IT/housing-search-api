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

    public string FixtureFilesPath = "V2/E2ETests/Fixtures/Files";
    private string _indexFilesPath = "data/elasticsearch";

    public ElasticsearchFixture()
    {
        var url = Environment.GetEnvironmentVariable("ELASTICSEARCH_DOMAIN_URL") ?? "http://localhost:9200";
        var settings = new ConnectionSettings(new Uri(url));
        Client = new ElasticClient(settings);
    }

    public void CreateIndex(string filename, string indexName)
    {
        var existsResponse = Client.Indices.Exists(indexName);
        if (existsResponse.Exists)
            Client.Indices.Delete(indexName);

        var indexDefinition = File.ReadAllText(Path.Combine(_indexFilesPath, filename));
        var response = Client.LowLevel.Indices.Create<StringResponse>(indexName, indexDefinition);

        if (!response.Success)
            throw new Exception("Failed to create index: " + response.DebugInformation);
        Console.WriteLine("Index created successfully.");
    }

    public void LoadData(string filename, string indexName)
    {
        Console.WriteLine($"Loading data from {filename} into index {indexName}");
        string jsonFilePath = Path.Combine(FixtureFilesPath, filename);
        if (!File.Exists(jsonFilePath))
            throw new FileNotFoundException($"The file {jsonFilePath} could not be found in directory {Directory.GetCurrentDirectory()}");

        string jsonContent = File.ReadAllText(jsonFilePath);

        // Perform the bulk insert with immediate refresh
        var bulkResponse = Client.LowLevel.Bulk<StringResponse>(
            PostData.String(jsonContent),
            new BulkRequestParameters { Refresh = Refresh.WaitFor }
        );

        if (!bulkResponse.Success)
            throw new Exception("Bulk insert failed: " + bulkResponse.DebugInformation);
    }

    public Task InitializeAsync()
    {
        var indexSettingsFiles = new string[] { "assetIndex.json", "tenureIndex.json", "personIndex.json" };

        foreach (string filename in indexSettingsFiles)
        {
            var indexName = filename.Replace("Index.json", "") + "s";
            CreateIndex(filename, indexName);
        }

        var filenames = new string[] { "assets.json", "tenures.json", "persons.json" };
        foreach (string filename in filenames)
        {
            var indexName = filename.Replace(".json", "");
            LoadData(filename, indexName);
        }

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
