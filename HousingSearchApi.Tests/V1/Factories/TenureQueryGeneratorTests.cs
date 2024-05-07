using AutoFixture;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Infrastructure.Factories;
using HousingSearchApi.V1.Interfaces;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using Xunit;

namespace HousingSearchApi.Tests.V1.Factories
{
    public class TenureQueryGeneratorTests
    {
        private readonly TenureQueryGenerator _sut;
        private readonly QueryContainerDescriptor<QueryableTenure> _queryContainerDescriptor;
        private readonly Mock<IQueryBuilder<QueryableTenure>> _queryBuilderMock;
        private readonly Mock<IFilterQueryBuilder<QueryableTenure>> _queryFilterBuilderMock;
        private readonly Fixture _fixure;
        private readonly GetTenureListRequest _request;

        public TenureQueryGeneratorTests()
        {
            _queryBuilderMock = new Mock<IQueryBuilder<QueryableTenure>>();
            _queryFilterBuilderMock = new Mock<IFilterQueryBuilder<QueryableTenure>>();
            _fixure = new Fixture();
            _request = _fixure.Build<GetTenureListRequest>().Create();
            _queryContainerDescriptor = new QueryContainerDescriptor<QueryableTenure>();
            _sut = new TenureQueryGenerator(_queryBuilderMock.Object, _queryFilterBuilderMock.Object);
        }

        [Fact]
        public void CreateThrowsArgumentNullExceptionWithAMessageWhenRequestIsNotGetTenureListRequestType()
        {
            //Arrange
            var request = new object();
            var expectedExceptionMessage = "Value cannot be null. (Parameter 'request shouldn't be null.')";

            //Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _sut.Create(request, _queryContainerDescriptor));
            Assert.Equal(expectedExceptionMessage, ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateThrowsArgumentNullExceptionWithAMessageWhenBothSearchTextAndUprnAreNullOrEmpty(string input)
        {
            //Arrange
            _request.SearchText = input;
            _request.Uprn = input;

            var expectedExceptionMessage = "Value cannot be null. (Parameter 'request should include seachText or Uprn.')";

            //Act and assert
            var ex = Assert.Throws<ArgumentNullException>(() => _sut.Create(_request, _queryContainerDescriptor));
            Assert.Equal(expectedExceptionMessage, ex.Message);
        }

        [Fact]
        public void CreateCallsQueryBuilderWhenSearchTextIsEmptyAndUprnIsProvided()
        {
            // Arrange
            _request.SearchText = "";

            _queryBuilderMock
                .Setup(x =>
                    x.WithExactQuery(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<IExactSearchQuerystringProcessor>(), It.IsAny<TextQueryType>())
                    .Build(It.IsAny<QueryContainerDescriptor<QueryableTenure>>())).Verifiable();

            // Act
            _sut.Create(_request, _queryContainerDescriptor);

            // Assert
            _queryBuilderMock.Verify();
        }

        [Fact]
        public void CreateCallsQueryFilterBuilderWhenSearchTextIsNotNullAndIsLongerThanZero()
        {
            // Arrange
            _queryFilterBuilderMock.Setup(x =>
                x.WithMultipleFilterQuery(It.IsAny<string>(), It.IsAny<List<string>>())
                .WithFilterQuery(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<TextQueryType>())
                .WithWildstarQuery(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<TextQueryType>())
                .Build(It.IsAny<QueryContainerDescriptor<QueryableTenure>>())).Verifiable();

            // Act
            _sut.Create(_request, _queryContainerDescriptor);

            //Assert
            _queryFilterBuilderMock.Verify();
        }

        [Fact]
        public void CreateGeneratesCorrectQueryWhenUsingWildstarQuery()
        {
            // Arrange
            var expectedMultipleFilterQueryFields = new List<string>()
            {
                 "tenuredAsset.isTemporaryAccommodation"
            };

            var expectedWithFilterQueryFields = new List<string>()
            {
                "tempAccommodationInfo.bookingStatus"
            };

            var expectedWildstarFields = new List<string>()
            {
                "paymentReference",
                "tenuredAsset.fullAddress^3",
                "householdMembers",
                "householdMembers.fullName^3"
            };

            var expectedTextQueryType = TextQueryType.MostFields;

            _queryFilterBuilderMock.Setup(x =>
                x.WithMultipleFilterQuery(It.IsAny<string>(), It.IsAny<List<string>>())
                .WithFilterQuery(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<TextQueryType>())
                .WithWildstarQuery(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<TextQueryType>())
                .Build(It.IsAny<QueryContainerDescriptor<QueryableTenure>>()));

            // Act
            _ = _sut.Create(_request, _queryContainerDescriptor);

            // Assert
            _queryFilterBuilderMock.Verify(x =>
                x.WithMultipleFilterQuery(_request.IsTemporaryAccommodation, expectedMultipleFilterQueryFields)
                .WithFilterQuery(_request.BookingStatus, expectedWithFilterQueryFields, It.IsAny<TextQueryType>())
                .WithWildstarQuery(_request.SearchText, expectedWildstarFields, expectedTextQueryType).Build(_queryContainerDescriptor), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateGeneratesCorrectQueryWhenUsingExactQuery(string searchText)
        {
            // Arrange
            _request.SearchText = searchText;

            var expectedExactQueryFields = new List<string>()
            {
                "tenuredAsset.uprn"
            };

            var expectedTextQueryType = TextQueryType.MostFields;

            _queryBuilderMock.Setup(x =>
                x.WithExactQuery(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<IExactSearchQuerystringProcessor>(), It.IsAny<TextQueryType>())
                .Build(It.IsAny<QueryContainerDescriptor<QueryableTenure>>()));

            // Act
            _ = _sut.Create(_request, _queryContainerDescriptor);

            // Assert
            _queryBuilderMock.Verify(x =>
                x.WithExactQuery(_request.Uprn, expectedExactQueryFields, null, expectedTextQueryType)
                .Build(_queryContainerDescriptor), Times.Once);
        }
    }
}
