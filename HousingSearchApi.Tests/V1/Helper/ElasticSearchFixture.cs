using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Threading;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class ElasticSearchFixture : IDisposable
    {
        private readonly MockWebApplicationFactory<Startup> _factory;

        public ElasticSearchFixture()
        {
            _factory = new MockWebApplicationFactory<Startup>();
            TestDataHelper.InsertPersonInEs(_factory.Services.GetService<IElasticClient>());

            // For the index to have time to be populated
            Thread.Sleep(500);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (null != _factory)
                    _factory.Dispose();
                _disposed = true;
            }
        }
    }
}
