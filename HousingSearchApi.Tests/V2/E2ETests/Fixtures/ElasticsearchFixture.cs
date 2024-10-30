using Nest;
using System;
using System.IO;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Xunit;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HousingSearchApi.Tests.V2.E2ETests.Fixtures;

public class ElasticsearchFixture : IAsyncLifetime
{
    public IElasticClient Client { get; private set; }
    public string FixtureFilesPath = "V2/E2ETests/Fixtures/Files";

    private string _indexFilesPath => "data/elasticsearch";

    public ElasticsearchFixture()
    {
        var url = Environment.GetEnvironmentVariable("ELASTICSEARCH_DOMAIN_URL") ?? "http://localhost:9200";
        var settings = new ConnectionSettings(new Uri(url));
        Client = new ElasticClient(settings);
    }

    public async Task CreateIndexAsync(string filename, string indexName)
    {
        var indexraw = await File.ReadAllTextAsync(Path.Combine(_indexFilesPath, filename));
        var index = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(indexraw);

        var createIndexResponse = await Client.Indices.CreateAsync(indexName, c => c
            .InitializeUsing(new IndexState
            {
                Settings = new IndexSettings(index["mappings"])
            })
        );

        if (!createIndexResponse.IsValid)
        {
            throw new Exception("Index creation failed: " + createIndexResponse.DebugInformation);
        }
    }

    public async Task LoadDataAsync(string filename)
    {
        string jsonFilePath = Path.Combine(FixtureFilesPath, filename);
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"The file {jsonFilePath} could not be found in directory {Directory.GetCurrentDirectory()}");
        }

        string jsonContent = await File.ReadAllTextAsync(jsonFilePath);

        var bulkResponse = await Client.LowLevel.BulkAsync<StringResponse>(PostData.String(jsonContent));

        if (!bulkResponse.Success)
        {
            throw new Exception("Bulk insert failed: " + bulkResponse.DebugInformation);
        }
    }

    public async Task InitializeAsync()
    {
        var indexSettingsFiles = new string[] { "assetIndex.json", "tenureIndex.json", "personIndex.json" };

        foreach (string filename in indexSettingsFiles)
        {
            await CreateIndexAsync(filename, indexName: filename.Replace("Index.json", ""));
        }

        string[] filenames = { "assets.json", "tenures.json", "persons.json" };

        foreach (string filename in filenames)
        {
            await LoadDataAsync(filename);
        }
    }

    public Task DisposeAsync()
    {
        // Clean up if necessary, e.g., deleting the index
        return Task.CompletedTask;
    }
}
