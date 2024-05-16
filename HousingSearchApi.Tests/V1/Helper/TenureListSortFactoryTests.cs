using FluentAssertions;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Infrastructure.Sorting;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class TenureListSortFactoryTests
    {
        private readonly SortFactory _sut;
        private static readonly string _tenureStartDateFieldName = "tenureStartDate";

        public TenureListSortFactoryTests()
        {
            _sut = new SortFactory();
        }

        [Fact]
        public void ShouldNotSortByDefault()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableTenure, GetTenureListRequest>(new GetTenureListRequest());

            // Assert
            result.Should().BeOfType(typeof(DefaultSort<QueryableTenure>));
        }

        [Fact]
        public void ShouldReturnTenureStartDateAscWhenRequestTenureStartDateAndAsc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableTenure, GetTenureListRequest>(new GetTenureListRequest { SortBy = _tenureStartDateFieldName, IsDesc = false });

            // Assert
            result.Should().BeOfType(typeof(TenureStartDateAsc));
        }

        [Fact]
        public void ShouldReturnTenureStartDateDescWhenRequestTenureStartDateAndDesc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableTenure, GetTenureListRequest>(new GetTenureListRequest { SortBy = _tenureStartDateFieldName, IsDesc = true });

            // Assert
            result.Should().BeOfType(typeof(TenureStartDateDesc));
        }
    }
}
