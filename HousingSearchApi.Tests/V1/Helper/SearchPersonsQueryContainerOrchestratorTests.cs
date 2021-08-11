using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using HousingSearchApi.V1.Interfaces;
using Nest;
using System.Linq;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class SearchPersonsQueryContainerOrchestratorTests
    {
        private SearchPersonsQueryContainerOrchestrator _sut;

        public SearchPersonsQueryContainerOrchestratorTests()
        {
            _sut = new SearchPersonsQueryContainerOrchestrator(new WildCardAppenderAndPrepender());
        }

        [Fact]
        public void ShouldReturnSearchPhraseQueryStringContainer()
        {
            // Arrange + Act
            var result = _sut.CreatePerson(new GetPersonListRequest { SearchText = "abc", PersonType = PersonType.Rent },
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

            iFilterByText.QueryString.Query.Should().Be("*abc*");

            iFilterByText.QueryString.Fields.Should().HaveCount(1);

            iFilterByText.QueryString.Fields.FirstOrDefault().Name.Should().Be("*");

            iFilterByText.QueryString.Type.Should().Be(TextQueryType.MostFields);
        }
    }
}
