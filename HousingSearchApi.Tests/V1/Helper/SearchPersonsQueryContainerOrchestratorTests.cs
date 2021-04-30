using System.Linq;
using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways;
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
            _sut = new SearchPersonsQueryContainerOrchestrator();
        }

        [Fact]
        public void ShouldReturnOrSearchNamesSurnames()
        {
            // Arrange + Act
            var result = _sut.Create(new GetPersonListRequest { SearchText = "abc" },
                new QueryContainerDescriptor<QueryablePerson>());

            // Assert
            (result as IQueryContainer).Bool.Should.Count().Should().Be(2);
        }
    }
}
