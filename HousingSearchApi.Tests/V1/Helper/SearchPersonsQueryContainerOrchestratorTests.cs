using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using HousingSearchApi.V1.Interfaces;
using Nest;
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
            var result = _sut.CreatePerson(new HousingSearchRequest { SearchText = "abc" },
                new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            (result as IQueryContainer).QueryString.Should().NotBeNull();
        }
    }
}
