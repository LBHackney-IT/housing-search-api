using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nest;
using System;
using System.Linq;
using HousingSearchApi.V1.Infrastructure.Extensions;
using Xunit;

namespace HousingSearchApi.Tests.V1.Infrastructure
{
    public class ElasticSearchExtensionsTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private const string ConfigKey = "ELASTICSEARCH_DOMAIN_URL";
        private const string EsNodeUrl = "http://localhost:9200";

        public ElasticSearchExtensionsTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            ConfigureConfig(_mockConfiguration, EsNodeUrl);
        }

        private void ConfigureConfig(Mock<IConfiguration> mockConfig, string url)
        {
            var section = new Mock<IConfigurationSection>();
            section.Setup(x => x.Key).Returns(ConfigKey);
            section.Setup(x => x.Value).Returns(url);
            mockConfig.Setup(x => x.GetSection(ConfigKey))
                              .Returns(section.Object);
            mockConfig.Setup(x => x.GetSection("USING_REMOTE_DB"))
                              .Returns(new Mock<IConfigurationSection>().Object);
        }

        [Fact]
        public void ConfigureElasticSearchTestNullServicesThrows()
        {
            Action act = () => ElasticSearchExtensions.ConfigureElasticSearch((IServiceCollection) null, _mockConfiguration.Object);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ConfigureElasticSearchTestNullConfigurationThrows()
        {
            Action act = () => ElasticSearchExtensions.ConfigureElasticSearch(new ServiceCollection(), null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(EsNodeUrl)]
        public void ConfigureElasticSearchTestRegistersServices(string url)
        {
            ConfigureConfig(_mockConfiguration, url);

            var services = new ServiceCollection();
            services.ConfigureElasticSearch(_mockConfiguration.Object);

            _mockConfiguration.Verify(x => x.GetSection(ConfigKey), Times.Once);

            var serviceProvider = services.BuildServiceProvider();
            var esClient = serviceProvider.GetService<IElasticClient>();
            esClient.Should().NotBeNull();
            esClient.ConnectionSettings.ConnectionPool.Nodes.Count.Should().Be(1);
            var expectedUrl = string.IsNullOrEmpty(url) ? "http://localhost:9200" : url;
            esClient.ConnectionSettings.ConnectionPool.Nodes.First().Uri.Should().BeEquivalentTo(new Uri(expectedUrl));
        }
    }
}
