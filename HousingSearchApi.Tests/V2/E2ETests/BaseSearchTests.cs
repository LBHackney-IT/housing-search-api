using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HousingSearchApi.Tests.V2.E2ETests.Fixtures;
using Microsoft.Extensions.Logging;

namespace HousingSearchApi.Tests.V2.E2ETests;

using Nest;
using Xunit;


[CollectionDefinition("V2.E2ETests Collection", DisableParallelization = true)]
public class V2E2ETestsCollection : ICollectionFixture<CombinedFixture>
{
    // This class is used only to define the collection and its settings.
}

[Collection("V2.E2ETests Collection")]
public class BaseSearchTests
{
    private readonly string _indexName;

    protected readonly Random Random = new();

    private readonly ILogger<BaseSearchTests> _logger = new Logger<BaseSearchTests>(new LoggerFactory());


    protected BaseSearchTests(CombinedFixture combinedFixture, string indexName)
    {
        _indexName = indexName;
    }

    protected HttpRequestMessage CreateSearchRequest(string searchText) =>
        new(
            HttpMethod.Get,
            $"http://localhost:3000/api/v2/search/{_indexName}/?searchText={searchText}"
        );

    protected JsonElement GetResponseRootElement(HttpResponseMessage response)
    {
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var doc = JsonDocument.Parse(responseBody);
        return doc.RootElement;
    }


    protected IElasticClient GetElasticsearch()
    {
        var elasticsearchHost = Environment.GetEnvironmentVariable("ELASTICSEARCH_DOMAIN_URL") ?? "http://localhost:9200";
        var settings = new ConnectionSettings(new Uri(elasticsearchHost));
        var client = new ElasticClient(settings);
        // check client connection
        var pingResult = client.Ping();
        if (!pingResult.IsValid)
            throw new Exception("Elasticsearch client connection failed.");

        return client;
    }


    protected JsonElement RandomItem()
    {
        var es = GetElasticsearch();
        var res = es.Search<Dictionary<string, object>>(s => s
            .Index(_indexName)
            .Query(q => q
                .FunctionScore(fs => fs
                    .Query(qq => qq.MatchAll())
                    .Functions(f => f
                        .RandomScore()
                    )
                    .BoostMode(FunctionBoostMode.Multiply)
                )
            )
            .Size(1)
        );
        var firstDoc = JsonSerializer.Serialize(res.Documents.First());
        return JsonDocument.Parse(firstDoc).RootElement;
    }

    /// <summary>
    /// Executes a given asynchronous action multiple times until the maximum number of attempts is exceeded and tracks successes.
    /// </summary>
    /// <param name="attempts">The number of attempts to execute the action.</param>
    /// <param name="action">The asynchronous action to be executed.</param>
    /// <returns>The number of successful executions.</returns>
    protected async Task<int> RunWithScore(int attempts, Func<Task> action)
    {
        var successCount = 0;

        for (int attempt = 0; attempt < attempts; attempt++)
        {
            try
            {
                await action();
                successCount++;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Attempt {attempt + 1} failed.");
            }
        }

        return successCount;
    }

    protected string CreateTypo(string text) {
        var chars = text.ToCharArray();
        var randomChar = Random.Next(0, 36);
        var charIndexes = chars.Select((c, i) => (c, i)).Where(t => char.IsLetterOrDigit(t.c)).Select(t => t.i).ToList();
        var randomIndex = Random.Next(0, charIndexes.Count);
        chars[charIndexes[randomIndex]] = (char)('a' + randomChar);
        return new string(chars);
    }
}
