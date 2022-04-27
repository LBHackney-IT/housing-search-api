using FluentAssertions;
using Hackney.Core.ElasticSearch;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
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
    public class AssetQueryGeneratorTests
    {
        private readonly AssetQueryGenerator _sut;
        private readonly Mock<IQueryBuilder<QueryableAsset>> _queryBuilderMock;
        private readonly WildCardAppenderAndPrepender _wildCardAppenderAndPrepender;

        public AssetQueryGeneratorTests()
        {
            _wildCardAppenderAndPrepender = new WildCardAppenderAndPrepender();
            _queryBuilderMock = new Mock<IQueryBuilder<QueryableAsset>>();
            _sut = new AssetQueryGenerator(_queryBuilderMock.Object);
        }

        [Fact]
        public void ShouldGenerateWildstarQueryWithBestFieldQueryType()
        {
            // Arrange
            var queryContainer = new QueryContainerDescriptor<QueryableAsset>();

            var request = new GetAssetListRequest
            {
                SearchText = "21 Loddiges House Loddiges Road",
                Page = 1,
                PageSize = 10,
                AssetTypes = "Dwelling"
            };

            _queryBuilderMock.Setup(x => x.WithWildstarQuery(It.IsAny<string>(), It.IsAny<List<string>>(), TextQueryType.BestFields))
                .Returns(new QueryBuilder<QueryableAsset>(_wildCardAppenderAndPrepender));

            // Act
            var result = _sut.Create<GetAssetListRequest>(request, queryContainer);

            // Assert

            _queryBuilderMock.Verify(x => x.WithWildstarQuery(It.IsAny<string>(), It.IsAny<List<string>>(), TextQueryType.BestFields), Times.Once);
            
        }

        [Fact]
        public void ShouldGenerateFilterQueryWithBestFieldQueryType()
        {
            // Arrange
            var queryContainer = new QueryContainerDescriptor<QueryableAsset>();

            var request = new GetAssetListRequest
            {
                SearchText = "21 Loddiges House Loddiges Road",
                Page = 1,
                PageSize = 10,
                AssetTypes = "Dwelling"
            };

            _queryBuilderMock.Setup(x => x.WithFilterQuery(It.IsAny<string>(), It.IsAny<List<string>>(), TextQueryType.BestFields))
                .Returns(new QueryBuilder<QueryableAsset>(_wildCardAppenderAndPrepender));

            // Act
            var result = _sut.Create<GetAssetListRequest>(request, queryContainer);

            // Assert

            _queryBuilderMock.Verify(x => x.WithFilterQuery(It.IsAny<string>(), It.IsAny<List<string>>(), TextQueryType.BestFields), Times.Once);
        }

        [Fact]
        public void ShouldGenerateExactQueryWithBestFieldQueryType()
        {
            // Arrange
            var queryContainer = new QueryContainerDescriptor<QueryableAsset>();

            var request = new GetAssetListRequest
            {
                SearchText = "21 Loddiges House Loddiges Road",
                Page = 1,
                PageSize = 10,
                AssetTypes = "Dwelling"
            };

            _queryBuilderMock.Setup(x => x.WithExactQuery(It.IsAny<string>(), It.IsAny<List<string>>(), null, TextQueryType.BestFields))
                .Returns(new QueryBuilder<QueryableAsset>(_wildCardAppenderAndPrepender));

            // Act
            var result = _sut.Create<GetAssetListRequest>(request, queryContainer);

            // Assert

            _queryBuilderMock.Verify(x => x.WithExactQuery(It.IsAny<string>(), It.IsAny<List<string>>(), null, TextQueryType.BestFields), Times.Once);

        }

        [Fact]
        public void ShouldThrowNullExceptionIfRequestIsNull()
        {
            var queryContainer = new QueryContainerDescriptor<QueryableAsset>();
            Func<Task> func = () => { _sut.Create<GetAssetListRequest>(null, queryContainer); return Task.CompletedTask; };
            func.Should().Throw<ArgumentNullException>();
        }
    }
}
