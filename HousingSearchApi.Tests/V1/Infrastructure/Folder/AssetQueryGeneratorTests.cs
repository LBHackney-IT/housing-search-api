using Hackney.Core.ElasticSearch;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Infrastructure.Factories;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.Infrastructure.Folder
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
        public void ShouldGenerateQueryWithoutAddressWildcard()
        {
            // Arrange
            var qc = new QueryContainerDescriptor<QueryableAsset>();

            var request = new GetAssetListRequest
            {
                ExactMatch = true,
                SearchText = "10 Allerdale Drive",
                Page = 1,
                PageSize = 10,
                AssetTypes = "Dwelling"
            };

            _queryBuilderMock.Setup(x => x.WithWildstarQuery(It.IsAny<string>(), It.IsAny<List<string>>()))
                .Returns(new QueryBuilder<QueryableAsset>(_wildCardAppenderAndPrepender));

            // Act
            var result = _sut.Create<GetAssetListRequest>(request, qc);

            // Assert
            var expectedList = new List<string> { "assetAddress.postCode", "assetAddress.uprn" };

            _queryBuilderMock.Verify(qbm => qbm.WithWildstarQuery(It.IsAny<string>(), expectedList), Times.Once);
        }
    }
}
