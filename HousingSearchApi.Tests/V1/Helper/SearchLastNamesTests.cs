using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Nest;
using Xunit;
using QueryablePerson = HousingSearchApi.V1.Domain.ElasticSearch.QueryablePerson;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class SearchSurnamesTests
    {
        private SearchSurnames _sut;

        public SearchSurnamesTests()
        {
            _sut = new SearchSurnames();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldReturnNullIfRequestSearchTextIsEmpty(string searchText)
        {
            // Arrange + Act
            var result = _sut.Create(new GetPersonListRequest { SearchText = searchText }, new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ShouldReturnQueryThatSearchesForNames()
        {
            // Arrange
            var nameToSearchFor = "SomeName";

            // Act
            var result = _sut.Create(new GetPersonListRequest { SearchText = nameToSearchFor },
                new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            (result as IQueryContainer).Wildcard.Field.Expression.Print().Should().Be("f => f.Surname");
            (result as IQueryContainer).Wildcard.Value.Should().Be($"*{nameToSearchFor.ToLower()}*");
        }
    }
}
