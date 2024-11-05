using Nest;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V2.E2ETests.Fixtures;

public class ElasticsearchFixture : IAsyncLifetime
{
    public IElasticClient Client { get; private set; }

    public ElasticsearchFixture()
    {
        var url = Environment.GetEnvironmentVariable("ELASTICSEARCH_DOMAIN_URL") ?? "http://localhost:9200";
        var settings = new ConnectionSettings(new Uri(url));
        Client = new ElasticClient(settings);
    }

    public Task InitializeAsync()
    {

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
