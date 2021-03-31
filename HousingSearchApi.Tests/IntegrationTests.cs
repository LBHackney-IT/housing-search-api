using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace HousingSearchApi.Tests
{
    public class IntegrationTests<TStartup> where TStartup : class
    {
        protected HttpClient Client { get; private set; }
        private MockWebApplicationFactory<TStartup> _factory;

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
    }
}
