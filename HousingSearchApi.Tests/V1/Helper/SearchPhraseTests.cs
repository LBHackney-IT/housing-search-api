using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using HousingSearchApi.V1.Interfaces;
using Nest;
using System.Linq;
using System.Linq.Expressions;
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
        public void ShouldReturnFilterOnlyByTypeIfRequestSearchTextIsEmpty(string searchText)
        {
            // Arrange + Act
            var result = _sut.CreatePersonQuery(new GetPersonListRequest { SearchText = searchText }, new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<QueryContainerDescriptor<QueryablePerson>>();
        }

        [Fact]
        public void ShouldReturnQueryThatSearchesForProvidedTextAndProvidedType()
        {
            // Arrange
            var nameToSearchFor = "SomeName LastName";
            var nameToExpect = "*SomeName* *LastName*";

            // Act
            var result = _sut.CreatePersonQuery(new GetPersonListRequest { SearchText = nameToSearchFor, PersonType = PersonType.Rent },
                new QueryContainerDescriptor<QueryablePerson>());

            // Assert

            var query = result as IQueryContainer;

            query.Should().NotBeNull();

            var searchByType = query.Bool.Must;

            searchByType.Should().HaveCount(1);

            var iSearchByType = searchByType.FirstOrDefault() as IQueryContainer;

            iSearchByType.Should().NotBeNull();

            iSearchByType.ConstantScore.Should().NotBeNull();

            var typeFilter = iSearchByType.ConstantScore.Filter;

            var iTypeFilter = typeFilter as IQueryContainer;

            iTypeFilter.Should().NotBeNull();

            iTypeFilter.Term.Value.Should().BeEquivalentTo("rent");

            query.Bool.Filter.Should().HaveCount(1);

            var iFilterByText = query.Bool.Filter.FirstOrDefault() as IQueryContainer;

            iFilterByText.Should().NotBeNull();

            iFilterByText.QueryString.Query.Should().Be(nameToExpect);

            iFilterByText.QueryString.Fields.Should().HaveCount(1);

            iFilterByText.QueryString.Fields.FirstOrDefault().Name.Should().Be("*");

            iFilterByText.QueryString.Type.Should().Be(TextQueryType.MostFields);
        }
    }
}
