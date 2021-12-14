using HousingSearchApi.V1.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HousingSearchApi.Tests
{
    public class MockWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private IConfiguration _configuration;
        private const string TestToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJ0ZXN0IiwiaWF0IjoxNjM5NDIyNzE4LCJleHAiOjE5ODY1Nzc5MTgsImF1ZCI6InRlc3QiLCJzdWIiOiJ0ZXN0IiwiZ3JvdXBzIjpbInNvbWUtdmFsaWQtZ29vZ2xlLWdyb3VwIiwic29tZS1vdGhlci12YWxpZC1nb29nbGUtZ3JvdXAiXSwibmFtZSI6InRlc3RpbmcifQ.IcpQ00PGVgksXkR_HFqWOakgbQ_PwW9dTVQu4w77tmU";

        public HttpClient Client { get; private set; }
        public IElasticClient ElasticSearchClient { get; private set; }

        public MockWebApplicationFactory()
        {
            EnsureEnvVarConfigured("REQUIRED_GOOGL_GROUPS", "some-valid-google-group");

            Client = CreateClient();
            Client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(TestToken);
        }

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

        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
            {
                Environment.SetEnvironmentVariable(name, defaultValue);
            }
        }
    }
}
