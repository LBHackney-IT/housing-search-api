using AutoFixture;
using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.Boundary.Requests
{
    public class GetTransactionListRequestTests
    {
        private readonly Fixture _fixture;
        public GetTransactionListRequestTests()
        {
            _fixture = new Fixture();
        }
        [Fact]
        public void RequestHasPropertiesSet()
        {
            GetTransactionListRequest request = _fixture.Create<GetTransactionListRequest>();
            var entityType = request.GetType();
            entityType.GetProperties().Length.Should().Be(8);

            Assert.IsType<bool>(request.IsDesc);
            Assert.IsType<int>(request.Page);
            Assert.IsType<int>(request.PageSize);
            Assert.IsType<string>(request.SearchText);
            Assert.IsType<string>(request.SortBy);
            Assert.IsType<Guid>(request.TargetId);
        }
    }
}
