using System;
using System.Collections.Generic;
using AutoFixture;

namespace HousingSearchApi.V1.Helper
{
    public static class MockParametersForValidator
    {
        private static readonly Fixture _fixture = new Fixture();

        public static List<object[]> GetTestData { get; } = new List<object[]>
        {
            new object[] {_fixture.Create<string>(), _fixture.Create<Guid>()},
            new object[] {string.Empty, _fixture.Create<Guid>()},
            new object[] {null, _fixture.Create<Guid>()},
            new object[] {_fixture.Create<string>(), Guid.Empty}
        };
    }
}
