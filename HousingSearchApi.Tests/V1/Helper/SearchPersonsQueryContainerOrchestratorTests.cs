using System.Linq;
using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Infrastructure;
using HousingSearchApi.V1.Interfaces;
using Moq;
using Nest;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class SearchPersonsQueryContainerOrchestratorTests
    {
        private SearchPersonsQueryContainerOrchestrator _sut;
        private Mock<IWildCardAppenderAndPrepender> _mockAppender;

        public SearchPersonsQueryContainerOrchestratorTests()
        {
            _mockAppender = new Mock<IWildCardAppenderAndPrepender>();

            _sut = new SearchPersonsQueryContainerOrchestrator(_mockAppender.Object);
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
