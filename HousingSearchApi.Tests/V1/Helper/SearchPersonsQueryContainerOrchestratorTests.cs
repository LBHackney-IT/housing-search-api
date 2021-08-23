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
            // Arrange
            var request = new GetPersonListRequest
            {
                SearchText = "abc",
                PersonType = PersonType.Rent
            };

            var expectedTypes = request.GetPersonTypes();

            // Act

            var result = _sut.CreatePerson(request, new QueryContainerDescriptor<QueryablePerson>());

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

            firstMustQuery.QueryString.Query.Should().BeEquivalentTo("*abc*");

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
