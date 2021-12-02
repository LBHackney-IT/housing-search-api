using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using Xunit;

namespace HousingSearchApi.Tests.V1.Boundary.Requests
{
    public class GetAccountListRequestTests
    {
        private readonly Fixture _fixture;
        public GetAccountListRequestTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void EntityHavePropertiesSet()
        {
            GetAccountListRequest entity = _fixture.Create<GetAccountListRequest>();

            var entityType = entity.GetType();
            entityType.GetProperties().Length.Should().Be(6);

            Assert.IsType<Guid>(entity.TargetId);
            Assert.IsType<int>(entity.Page);
            Assert.IsType<string>(entity.SearchText);
            Assert.IsType<bool>(entity.IsDesc);
            Assert.IsType<int>(entity.PageSize);
            Assert.IsType<string>(entity.SortBy);
        }
    }
}
