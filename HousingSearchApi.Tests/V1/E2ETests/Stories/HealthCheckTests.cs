using HousingSearchApi.Tests.V1.E2ETests.Steps;
using System;
using TestStack.BDDfy;
using Xunit;

namespace HousingSearchApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Api client",
        IWant = "to be able to validate that the service status is healty",
        SoThat = "I can be sure that calls made to it will succeed.")]
    [Collection("ElasticSearch collection")]
    public class HealthCheckTests : IDisposable
    {
        private readonly MockWebApplicationFactory<Startup> _factory;
        private readonly HealthCheckSteps _steps;

        public HealthCheckTests(MockWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            var client = _factory.CreateClient();

            _steps = new HealthCheckSteps(client);
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
                _disposed = true;
            }
        }

        [Fact]
        public void ServiceReturnsHealthyStatus()
        {
            this.Given("A running service")
                .When(w => _steps.WhenTheHealtchCheckIsCalled())
                .Then(t => _steps.ThenTheHealthyStatusIsReturned())
                .BDDfy();
        }
    }
}
