using System;
using FluentAssertions;
using HousingSearchApi.V1.Infrastructure;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class PagingHelperTests
    {
        private PagingHelper _sut;

        public PagingHelperTests()
        {
            _sut = new PagingHelper();
        }

        [Fact]
        public void ShouldReturnZeroIfCurrentPageIsZero()
        {
            // Arrange + Act
            var result = _sut.GetPageOffset((int) new Random(0).NextDouble(), 0);

            // Assert
            result.Should().Be(0);
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 20)]
        public void ShouldCalculateTheCorrectOffsetWhenCurrentPageNotZero(int currentPage, int pageSize)
        {
            // Arrange + Act
            var result = _sut.GetPageOffset(pageSize, currentPage);

            // Assert
            result.Should().Be((currentPage - 1) * pageSize);
        }
    }
}
