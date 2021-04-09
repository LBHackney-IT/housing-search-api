using FluentAssertions;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Infrastructure;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class PersonListSortFactoryTests
    {
        private PersonListSortFactory _sut;

        public PersonListSortFactoryTests()
        {
            _sut = new PersonListSortFactory();
        }

        [Fact]
        public void ShouldNotSortAsDefault()
        {
            // Arrange + Act
            var result = _sut.Create(new GetPersonListRequest());

            // Assert
            result.Should().BeOfType(typeof(DefaultSort));
        }

        [Fact]
        public void ShouldReturnLastNameAscWhenRequestLastNameAndAsc()
        {
            // Arrange + Act
            var result = _sut.Create(new GetPersonListRequest { SortBy = "lastname", IsDesc = false });

            // Assert
            result.Should().BeOfType(typeof(LastNameAsc));
        }

        [Fact]
        public void ShouldReturnLastNameDescWhenRequestLastNameAndDesc()
        {
            // Arrange + Act
            var result = _sut.Create(new GetPersonListRequest { SortBy = "lastname", IsDesc = true });

            // Assert
            result.Should().BeOfType(typeof(LastNameDesc));
        }
    }
}
