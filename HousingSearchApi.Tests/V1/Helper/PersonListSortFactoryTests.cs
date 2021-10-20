using FluentAssertions;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Sorting;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class PersonListSortFactoryTests
    {
        private SortFactory _sut;

        public PersonListSortFactoryTests()
        {
            _sut = new SortFactory();
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
