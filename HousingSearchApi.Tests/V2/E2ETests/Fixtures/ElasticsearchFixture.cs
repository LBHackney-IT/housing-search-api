using Nest;
using System;
using System.IO;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Xunit;
using System.Collections.Generic;

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
        var client = new ElasticClient();

        var existsResponse = client.Indices.Exists(indexName);
        if (existsResponse.Exists)
            client.Indices.Delete(indexName);

        var indexDefinition = File.ReadAllText(Path.Combine(_indexFilesPath, filename));
        var response = client.LowLevel.Indices.Create<StringResponse>(indexName, indexDefinition);

        if (!response.Success)
            throw new Exception("Failed to create index: " + response.DebugInformation);
        Console.WriteLine("Index created successfully.");
    }

    public async Task LoadDataAsync(string filename)
    {
        string jsonFilePath = Path.Combine(FixtureFilesPath, filename);
        if (!File.Exists(jsonFilePath))
            throw new FileNotFoundException($"The file {jsonFilePath} could not be found in directory {Directory.GetCurrentDirectory()}");


        string jsonContent = await File.ReadAllTextAsync(jsonFilePath);

        var bulkResponse = await Client.LowLevel.BulkAsync<StringResponse>(PostData.String(jsonContent));

        if (!bulkResponse.Success)
            throw new Exception("Bulk insert failed: " + bulkResponse.DebugInformation);

    }

    public async Task InitializeAsync()
    {
        var indexSettingsFiles = new string[] { "assetIndex.json", "tenureIndex.json", "personIndex.json" };

        foreach (string filename in indexSettingsFiles)
        {
            var indexName = filename.Replace("Index.json", "") + "s";
            CreateIndex(filename, indexName);
        }

        var filenames = new string[] { "assets.json", "tenures.json", "persons.json" };
        foreach (string filename in filenames)
            await LoadDataAsync(filename);
    }

    public Task DisposeAsync()
    {
        // Clean up if necessary, e.g., deleting the index
        return Task.CompletedTask;
    }
}
