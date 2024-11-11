using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V2.E2ETests.Fixtures;

public class CombinedFixture : IAsyncLifetime
{
    public MockWebApplicationFactory<Startup> Factory { get; private set; }
    public ElasticsearchFixture Elasticsearch { get; private set; }

    public readonly HttpClient HttpClient;

    public CombinedFixture()
    {
        Factory = new MockWebApplicationFactory<Startup>();
        Elasticsearch = new ElasticsearchFixture();
        HttpClient = Factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await Elasticsearch.InitializeAsync();
    }

    public Task DisposeAsync()
    {
        // Run teardown logic if necessary
        return Task.WhenAll(
            Factory.DisposeAsync().AsTask(),
            Elasticsearch.DisposeAsync()
        );
    }
}
