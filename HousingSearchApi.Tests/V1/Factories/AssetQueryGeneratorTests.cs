using Xunit;
using HousingSearchApi.V1.Infrastructure.Factories;
using Moq;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Core.ElasticSearch.Interfaces;
using HousingSearchApi.V1.Interfaces;
using System.Collections.Generic;
using HousingSearchApi.V1.Boundary.Requests;

namespace HousingSearchApi.Tests.V1.Factories
{
    public class AssetQueryGeneratorTests
    {
        //private readonly Fixture _fixture = new Fixture();

        private readonly AssetQueryGenerator _sut;
        private readonly Mock<IQueryBuilder<QueryableAsset>> _queryBuilderMock;
        private readonly Mock<IFilterQueryBuilder<QueryableAsset>> _queryFilterBuilder;

        public AssetQueryGeneratorTests()
        {
            _queryBuilderMock = new Mock<IQueryBuilder<QueryableAsset>>();
            _queryFilterBuilder = new Mock<IFilterQueryBuilder<QueryableAsset>>();

            _sut = new AssetQueryGenerator(_queryBuilderMock.Object, _queryFilterBuilder.Object);
        }

        [Fact]
        public void GeneratesCorrectQueryWhenUsingSimpleQuery()
        {
            (Nest.QueryContainerDescriptor<QueryableAsset>, string, List<string>) paramsCalled = (null, "", null);

            // Arrange
            _queryBuilderMock.Setup(x => x.BuildSimpleQuery(It.IsAny<Nest.QueryContainerDescriptor<QueryableAsset>>(), It.IsAny<string>(), It.IsAny<List<string>>()))
                .Callback<Nest.QueryContainerDescriptor<QueryableAsset>, string, List<string>>((q, s, l) => paramsCalled = (q, s, l));

            var request = new GetAssetListRequest
            {
                SearchText = "12 Pitcairn",
                IsSimpleQuery = true
            };

            var qcd = new Nest.QueryContainerDescriptor<QueryableAsset>();

            // Act
            _ = _sut.Create<GetAssetListRequest>(request, qcd);

            // Assert
            Assert.Equal(request.SearchText, paramsCalled.Item2);
            _queryBuilderMock.Verify(x => x.BuildSimpleQuery(It.IsAny<Nest.QueryContainerDescriptor<QueryableAsset>>(), It.IsAny<string>(), It.IsAny<List<string>>()), Times.Once);
        }
    }
}
