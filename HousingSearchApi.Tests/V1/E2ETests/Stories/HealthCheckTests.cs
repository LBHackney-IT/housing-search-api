using HousingSearchApi.Tests.V1.E2ETests.Steps;
using System;
using System.Net.Http;
using System.Threading;
using TestStack.BDDfy;
using Xunit;

namespace HousingSearchApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Api client",
        IWant = "to be able to validate that the service status is healty",
        SoThat = "I can be sure that calls made to it will succeed.")]
    public class HealthCheckTests : IDisposable
    {
        private readonly HttpClient _client;
        private readonly MockWebApplicationFactory<Startup> _factory;
        private readonly HealthCheckSteps _steps;

        public HealthCheckTests()
        {
            _factory = new MockWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
            _steps = new HealthCheckSteps(_client);

            Thread.Sleep(500);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _client.Dispose();
                _factory.Dispose();
                _disposed = true;
            }
        }

        [Fact(Skip = "Integration tests currently not working")]
        public void ServiceReturnsHealthyStatus()
        {
            this.Given("A running service")
                .When(w => _steps.WhenTheHealtchCheckIsCalled())
                .Then(t => _steps.ThenTheHealthyStatusIsReturned())
                .BDDfy();
        }
    }
}
