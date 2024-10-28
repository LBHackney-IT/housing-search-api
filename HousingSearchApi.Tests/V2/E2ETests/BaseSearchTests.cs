using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using HousingSearchApi.Tests.V2.E2ETests.Fixtures;
using Nest;
using Xunit;

namespace HousingSearchApi.Tests.V2.E2ETests;

public class BaseSearchTests : IClassFixture<CombinedFixture>
{
    private readonly string _fixtureFilePath;
    private readonly string _indexName;

    protected BaseSearchTests(CombinedFixture combinedFixture, string indexName)
    {
        _indexName = indexName;
        _fixtureFilePath = Path.Combine(combinedFixture.Elasticsearch.FixtureFilesPath, $"{indexName}.json");
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

    // Return a random item from the fixture json file
    protected JsonElement RandomItem()
    {
        using StreamReader r = new StreamReader(_fixtureFilePath);
        string json = r.ReadToEnd();
        List<string> splitLines = new List<string>(json.Split("\n"))
            .Where(line => !line.Contains("index") && !string.IsNullOrWhiteSpace(line)
            ).ToList();

        JsonDocument TryParse(string strJson)
        {
            try
            {
                return JsonDocument.Parse(strJson);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        var items = splitLines.Select(line => TryParse(line)?.RootElement).Where(x => x != null);
        var jsonElements = items as JsonElement?[] ?? items.ToArray();
        var item = jsonElements.ElementAt(new Random().Next(jsonElements.Count()));
        return (JsonElement) item;
    }
}
