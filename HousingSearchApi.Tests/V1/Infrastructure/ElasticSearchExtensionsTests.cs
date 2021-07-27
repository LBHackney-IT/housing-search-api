using FluentAssertions;
using HousingSearchApi.V1.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Linq;
using Xunit;

namespace HousingSearchApi.Tests.V1.Infrastructure
{
    public class ElasticSearchExtensionsTests
    {
        private const string EnvVarKey = "ELASTICSEARCH_DOMAIN_URL";
        private const string EsNodeUrl = "http://somedomain:9200";

        public ElasticSearchExtensionsTests()
        {
            Environment.SetEnvironmentVariable(EnvVarKey, null);
        }

        private void ConfigureEnvVar(string url)
        {
            Environment.SetEnvironmentVariable(EnvVarKey, url);
        }

        [Fact]
        public void ConfigureElasticSearchTestNullServicesThrows()
        {
            Action act = () => ElasticSearchExtensions.ConfigureElasticSearch((IServiceCollection) null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(EsNodeUrl)]
        public void ConfigureElasticSearchTestRegistersServices(string url)
        {
            ConfigureEnvVar(url);

            var services = new ServiceCollection();
            services.ConfigureElasticSearch();

            var serviceProvider = services.BuildServiceProvider();
            var esClient = serviceProvider.GetService<IElasticClient>();
            esClient.Should().NotBeNull();
            esClient.ConnectionSettings.ConnectionPool.Nodes.Count.Should().Be(1);
            var expectedUrl = string.IsNullOrEmpty(url) ? "http://localhost:9200" : url;
            esClient.ConnectionSettings.ConnectionPool.Nodes.First().Uri.Should().BeEquivalentTo(new Uri(expectedUrl));
        }
    }
}
