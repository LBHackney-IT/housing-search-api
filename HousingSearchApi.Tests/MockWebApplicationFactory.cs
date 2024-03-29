using HousingSearchApi.V1.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace HousingSearchApi.Tests
{
    public class MockWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private IConfiguration _configuration;

        public IElasticClient ElasticSearchClient { get; private set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(b =>
                    {
                        b.AddEnvironmentVariables();
                        _configuration = b.Build();
                    })
                   .UseStartup<Startup>();

            builder.ConfigureServices(services =>
            {
                services.ConfigureElasticSearch(_configuration);

                var serviceProvider = services.BuildServiceProvider();
                ElasticSearchClient = serviceProvider.GetRequiredService<IElasticClient>();
            });
        }
    }
}
