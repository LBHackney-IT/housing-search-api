using FluentAssertions;
using HousingSearchApi.V2.Gateways;
using Nest;
using System.Linq;
using Xunit;

namespace HousingSearchApi.Tests.V2.Gateways
{
    public class SearchOperationsTests
    {
        [Theory]
        [InlineData("12/A", "*12\\/A*")]
        [InlineData("Flat (A)", "*Flat* AND *\\(A\\)*")]
        [InlineData("double  space", "*double* AND *space*")]
        public void WildcardQueryStringQuery_EscapesAndSplitsCorrectly(string input, string expectedQuery)
        {
            // Arrange
            Nest.Fields fields = new[] { "field1" };

            // Act
            var queryFunc = SearchOperations.WildcardQueryStringQuery(input, fields);
            var container = queryFunc(new QueryContainerDescriptor<object>());
            var queryStringQuery = ((IQueryContainer) container).QueryString;

            // Assert
            queryStringQuery.Query.Should().Be(expectedQuery);
        }

        [Fact]
        public void WildcardMatch_HandlesMultipleSpacesCorrectly()
        {
            // Arrange
            Nest.Fields fields = new[] { "field1" };
            var input = "double  space";

            // Act
            var queryFunc = SearchOperations.WildcardMatch(input, fields, 1);
            var container = queryFunc(new QueryContainerDescriptor<object>());
            var boolQuery = ((IQueryContainer) container).Bool;

            // Assert
            boolQuery.Should.Count().Should().Be(2);
        }
    }
}
