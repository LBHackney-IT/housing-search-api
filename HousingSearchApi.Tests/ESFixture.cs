using System;
using System.Threading;
using HousingSearchApi.Tests.V1.Helper;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace HousingSearchApi.Tests
{
    public class ESFixture : IDisposable
    {
        private MockWebApplicationFactory<Startup> _factory;

        public ESFixture()
        {
            _factory = new MockWebApplicationFactory<Startup>();
            TestDataHelper.InsertPersonInEs(_factory.Services.GetService<IElasticClient>());

            // For the index to have time to be populated
            Thread.Sleep(500);
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}
