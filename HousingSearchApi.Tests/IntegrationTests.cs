using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace HousingSearchApi.Tests
{
    public class IntegrationTests<TStartup> where TStartup : class
    {
        protected HttpClient Client { get; private set; }
        private MockWebApplicationFactory<TStartup> _factory;
        private DockerClient _dockerClient;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            _dockerClient = new DockerClientConfiguration()
                .CreateClient();

            var parameters = new ContainersListParameters();
            parameters.All = true;

            ContainerListResponse container = (await _dockerClient.Containers.ListContainersAsync(parameters)).FirstOrDefault(x => x.Names.Contains("test-elasticsearch"));
            if (container.Status.Contains("stop"))
            {
                await _dockerClient.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
            }
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {

        }

        [SetUp]
        public void BaseSetup()
        {
            _factory = new MockWebApplicationFactory<TStartup>();
            Client = _factory.CreateClient();
        }

        [TearDown]
        public void BaseTearDown()
        {
            Client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task WhenRequestDoesNotContainSearchStringShouldReturnBadRequestResult()
        {
            // arrange + act
            var response = await Client.GetAsync("api/v1/search/persons");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task WhenRequestContainsSearchStringShouldReturn200()
        {
            // arrange + act
            var response = await Client.GetAsync("api/v1/search/persons?searchText=abc");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
