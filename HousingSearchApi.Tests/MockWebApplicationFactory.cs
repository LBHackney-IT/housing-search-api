using HousingSearchApi.V1.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace HousingSearchApi.Tests
{
    public class MockWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        public IElasticClient ElasticSearchClient { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();

            builder.ConfigureServices(services =>
            {
                services.ConfigureElasticSearch();

                var serviceProvider = services.BuildServiceProvider();
                ElasticSearchClient = serviceProvider.GetRequiredService<IElasticClient>();
            });
        }
    }
}
