using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using HousingSearchApi.V1.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Nest;
using Xunit;


namespace HousingSearchApi.Tests.V1.Helper
{
    public class SearchPhraseTests
    {
        private readonly SearchPhrase _sut;

        public SearchPhraseTests()
        {
            _sut = new SearchPhrase(new WildCardAppenderAndPrepender());
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
            var nameToSearchFor = "SomeName LastName";
            var nameToExpect = "*SomeName* *LastName*";

            // Act
            var result = _sut.Create(new GetPersonListRequest { SearchText = nameToSearchFor },
                new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            (result as IQueryContainer).QueryString.Query.Should().Be(nameToExpect);
            (result as IQueryContainer).QueryString.Type.Should().Be(TextQueryType.MostFields);
        }
    }
}
