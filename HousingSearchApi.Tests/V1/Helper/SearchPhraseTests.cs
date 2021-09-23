using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Gateways.Models.Persons;
using HousingSearchApi.V1.Infrastructure;
using HousingSearchApi.V1.Interfaces;
using Nest;
using System.Linq;
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
        [InlineData("AnyData")]
        [InlineData(null)]
        public void ShouldReturnNullIfRequestTypeIsUnknown(string searchText)
        {
            // Arrange + Act
            var result = _sut.Create(new HousingSearchRequest { SearchText = searchText }, new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("AnyData", "(*AnyData*) *AnyData*")]
        [InlineData("SomeName LastName", "(*SomeName* AND *LastName*) *SomeName* *LastName*")]
        public void ShouldReturnQueryThatSearchesForProvidedText(string searchText, string wildCardedSearchText)
        {
            // Arrange + Act
            var result = _sut.Create(new GetPersonListRequest { SearchText = searchText }, new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            result.Should().NotBeNull();

            var query = result as IQueryContainer;
            query.Should().NotBeNull();

            var searchFilter = query.Bool.Filter.ToList();

            searchFilter.Should().HaveCount(1);

            var filterByText = searchFilter[0] as IQueryContainer;

            filterByText.Should().NotBeNull();
            filterByText.QueryString.Query.Should().BeEquivalentTo(wildCardedSearchText);
            filterByText.QueryString.Fields.Should().HaveCount(2);
            filterByText.QueryString.Type.Should().Be(TextQueryType.MostFields);
        }

        [Theory]
        [InlineData(PersonType.Tenant)]
        [InlineData(PersonType.Leaseholder)]
        public void ShouldReturnQueryThatSearchesForProvidedTextAndProvidedType(PersonType type)
        {
            // Arrange
            var nameToSearchFor = "SomeName LastName";
            var nameToExpect = "(*SomeName* AND *LastName*) *SomeName* *LastName*";
            var expectedTypes = type.GetPersonTypes();

            // Act
            var result = _sut.Create(new GetPersonListRequest { SearchText = nameToSearchFor, PersonType = type },
                new QueryContainerDescriptor<QueryablePerson>());

            result.Should().NotBeNull();

            // Assert
            var query = result as IQueryContainer;

            query.Should().NotBeNull();

            var searchFilter = query.Bool.Filter.ToList();

            searchFilter.Should().HaveCount(2);

            var filterByText = searchFilter[0] as IQueryContainer;

            filterByText.Should().NotBeNull();
            filterByText.QueryString.Query.Should().BeEquivalentTo(nameToExpect);
            filterByText.QueryString.Fields.Should().HaveCount(2);
            filterByText.QueryString.Type.Should().Be(TextQueryType.MostFields);

            var filterByPersonType = searchFilter[1] as IQueryContainer;

            filterByPersonType.QueryString.Query.Should().BeEquivalentTo(string.Join(' ', expectedTypes));
            filterByPersonType.QueryString.Fields.Should().HaveCount(1);
            filterByPersonType.QueryString.Fields.FirstOrDefault().Name.Should().Be("tenures.type");
            filterByPersonType.QueryString.Type.Should().Be(TextQueryType.MostFields);
        }
    }
}
