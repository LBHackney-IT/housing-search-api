using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Interfaces.Sorting;
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
            var result = _sut.Create<QueryablePerson>(new HousingSearchRequest());

            // Assert
            result.Should().BeOfType(typeof(DefaultSort<QueryablePerson>));
        }

        [Fact]
        public void ShouldReturnSurnameAscWhenRequestSurnameAndAsc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryablePerson>(new HousingSearchRequest { SortBy = "surname", IsDesc = false });

            // Assert
            result.Should().BeOfType(typeof(SurnameAsc));
        }

        [Fact]
        public void ShouldReturnSurnameDescWhenRequestSurnameAndDesc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryablePerson>(new HousingSearchRequest { SortBy = "surname", IsDesc = true });

            // Assert
            result.Should().BeOfType(typeof(SurnameDesc));
        }
    }
}
