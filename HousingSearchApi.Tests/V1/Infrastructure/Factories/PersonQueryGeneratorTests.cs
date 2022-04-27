using FluentAssertions;
using Hackney.Core.ElasticSearch;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Domain;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Infrastructure.Factories;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.Infrastructure.Factories
{
    public class PersonQueryGeneratorTests
    {
        private readonly PersonQueryGenerator _sut;
        private readonly Mock<IQueryBuilder<QueryablePerson>> _queryBuilderMock;
        private readonly Mock<IExactSearchQuerystringProcessor> _mockExactSearchProcessor;
        private readonly WildCardAppenderAndPrepender _wildCardAppenderAndPrepender;

        public PersonQueryGeneratorTests()
        {
            _queryBuilderMock = new Mock<IQueryBuilder<QueryablePerson>>();
            _mockExactSearchProcessor = new Mock<IExactSearchQuerystringProcessor>();
            _wildCardAppenderAndPrepender = new WildCardAppenderAndPrepender();
            _sut = new PersonQueryGenerator(_queryBuilderMock.Object);
        }


        [Fact]
        public void ShouldGenerateWildstarQueryWithMostFieldQueryType()
        {
            // Arrange
            var queryContainer = new QueryContainerDescriptor<QueryablePerson>();

            var request = new GetPersonListRequest
            {
                SearchText = "Fake",
                Page = 1,
                PageSize = 10,
                PersonType = PersonType.Leaseholder
            };

            _queryBuilderMock.Setup(x => x.WithWildstarQuery(It.IsAny<string>(), It.IsAny<List<string>>(), TextQueryType.MostFields))
                .Returns(new QueryBuilder<QueryablePerson>(_wildCardAppenderAndPrepender));

            // Act
            var result = _sut.Create<GetPersonListRequest>(request, queryContainer);

            // Assert

            _queryBuilderMock.Verify(x => x.WithWildstarQuery(It.IsAny<string>(), It.IsAny<List<string>>(), TextQueryType.MostFields), Times.Once);

        }

        [Fact]
        public void ShouldGenerateExactQueryWithMostFieldQueryType()
        {
            // Arrange
            var queryContainer = new QueryContainerDescriptor<QueryablePerson>();

            var request = new GetPersonListRequest
            {
                SearchText = "Fake",
                Page = 1,
                PageSize = 10
            };

            _queryBuilderMock.Setup(x => x.WithExactQuery(It.IsAny<string>(), It.IsAny<List<string>>(), null, TextQueryType.MostFields))
                .Returns(new QueryBuilder<QueryablePerson>(_wildCardAppenderAndPrepender));

            // Act
            var result = _sut.Create<GetPersonListRequest>(request, queryContainer);

            // Assert

            _queryBuilderMock.Verify(x => x.WithExactQuery(It.IsAny<string>(), It.IsAny<List<string>>(),null, TextQueryType.MostFields), Times.Once);

        }

        [Fact]
        public void ShouldGenerateFilterQueryWithMostFieldQueryType()
        {
            // Arrange
            var queryContainer = new QueryContainerDescriptor<QueryablePerson>();

            var request = new GetPersonListRequest
            {
                SearchText = "Fake",
                Page = 1,
                PageSize = 10,
                PersonType = PersonType.Leaseholder
            };

            _queryBuilderMock.Setup(x => x.WithExactQuery(It.IsAny<string>(), It.IsAny<List<string>>(), null, TextQueryType.MostFields))
               .Returns(new QueryBuilder<QueryablePerson>(_wildCardAppenderAndPrepender));

            // Act
            _sut.Create<GetPersonListRequest>(request, queryContainer);

            _queryBuilderMock.Setup(x => x.WithFilterQuery(It.IsAny<string>(), It.IsAny<List<string>>(), TextQueryType.MostFields))
                .Returns(new QueryBuilder<QueryablePerson>(_wildCardAppenderAndPrepender));

            // Act
            var result = _sut.Create<GetPersonListRequest>(request, queryContainer);

            // Assert

            _queryBuilderMock.Verify(x => x.WithFilterQuery(It.IsAny<string>(), It.IsAny<List<string>>(), TextQueryType.MostFields), Times.Once);

        }

        [Fact]
        public void ShouldThrowNullExceptionIfRequestIsNull()
        {
            var queryContainer = new QueryContainerDescriptor<QueryablePerson>();
            Func<Task> func = () => { _sut.Create<GetPersonListRequest>(null, queryContainer); return Task.CompletedTask; };
            func.Should().Throw<ArgumentNullException>();
        }
    }
}
