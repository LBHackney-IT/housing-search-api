using FluentAssertions;
using HousingSearchApi.V1.Logging;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace HousingSearchApi.Tests.V1.Logging
{
    [TestFixture]
    public class LogCallAttributeTests
    {
        [Test]
        public void DefaultConstructorTestSetsLogLevelTrace()
        {
            var sut = new LogCallAttribute();
            sut.Level.Should().Be(LogLevel.Trace);
        }
    }
}
