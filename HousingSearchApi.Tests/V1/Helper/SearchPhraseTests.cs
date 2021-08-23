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
        private readonly PersonQueryGenerator _sut;

        public SearchPhraseTests()
        {
            _sut = new PersonQueryGenerator(new WildCardAppenderAndPrepender());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldReturnFilterOnlyByTypeIfRequestSearchTextIsEmpty(string searchText)
        {
            // Arrange + Act
            var result = _sut.Create(new GetPersonListRequest { SearchText = searchText }, new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<QueryContainerDescriptor<QueryablePerson>>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("AnyData")]
        [InlineData(null)]
        public void ShouldReturnNullIfRequestIsBaseType(string searchText)
        {
            // Arrange + Act
            var result = _sut.Create(new HousingSearchRequest { SearchText = searchText }, new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ShouldReturnQueryThatSearchesForProvidedTextAndProvidedType()
        {
            // Arrange
            var nameToSearchFor = "SomeName LastName";
            var nameToExpect = "*SomeName* *LastName*";
            var expectedTypes = PersonType.Leaseholder.GetPersonTypes();

            // Act
            var result = _sut.Create(new GetPersonListRequest { SearchText = nameToSearchFor, PersonType = PersonType.Leaseholder },
                new QueryContainerDescriptor<QueryablePerson>());

            // Assert

            var query = result as IQueryContainer;

            query.Should().NotBeNull();

            var searchFilter = query.Bool.Filter;

            searchFilter.Should().HaveCount(1);

            var queryFilter = searchFilter.FirstOrDefault() as IQueryContainer;

            queryFilter.Should().NotBeNull();

            queryFilter.Bool.Should().NotBeNull();

            var mustValues = queryFilter.Bool.Must;

            mustValues.Should().HaveCount(2);

            var listFilters = mustValues.ToList();

            var firstMustQuery = listFilters[0] as IQueryContainer;

            firstMustQuery.QueryString.Query.Should().BeEquivalentTo(nameToExpect);

            firstMustQuery.QueryString.Fields.Should().HaveCount(1);

            firstMustQuery.QueryString.Fields.FirstOrDefault().Name.Should().Be("*");

            firstMustQuery.QueryString.Type.Should().Be(TextQueryType.MostFields);

            var secondMustQuery = listFilters[1] as IQueryContainer;

            secondMustQuery.QueryString.Query.Should().BeEquivalentTo(string.Join(' ', expectedTypes));

            secondMustQuery.QueryString.Fields.Should().HaveCount(1);

            secondMustQuery.QueryString.Type.Should().Be(TextQueryType.MostFields);
        }
    }
}
