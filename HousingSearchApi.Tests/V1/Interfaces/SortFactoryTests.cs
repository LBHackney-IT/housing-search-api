using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Sorting;
using Xunit;

namespace HousingSearchApi.Tests.V1.Interfaces
{
    public class SortFactoryTests
    {
        private SortFactory _sut;

        public SortFactoryTests()
        {
            _sut = new SortFactory();
        }

        [Fact]
        public void GivenARequestShouldReturnDefaultSortForUnknownType()
        {
            // Arrange + act
            var result = _sut.Create<SomeUnknownType>(new HousingSearchRequest());

            // Assert
            result.Should().BeOfType<DefaultSort<SomeUnknownType>>();
        }
    }

    public class SomeUnknownType
    {

    }
}
