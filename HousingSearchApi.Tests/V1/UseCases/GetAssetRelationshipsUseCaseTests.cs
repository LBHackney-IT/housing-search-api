using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Asset;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.UseCase;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.UseCases
{
    public class GetAssetRelationshipsUseCaseTests
    {
        private readonly GetAssetRelationshipsUseCase _sut;
        private readonly Mock<ISearchGateway> _searchGatewayMock;
        public GetAssetRelationshipsUseCaseTests()
        {
            _searchGatewayMock = new Mock<ISearchGateway>();

            _sut = new GetAssetRelationshipsUseCase(_searchGatewayMock.Object);
        }

        [Fact]
        public async Task ShouldReturnValidResponseObject()
        {
            // Arrange
            string parentId = Guid.NewGuid().ToString();

            var assetList = new List<Asset>
            {
                new Asset
                {
                    AssetId = "TEST0001",
                    ParentAssetIds = parentId
                },
                new Asset
                {
                    AssetId = "TEST0002",
                    ParentAssetIds = parentId
                }
            };

            _searchGatewayMock.Setup(x => x.GetChildAssets(Moq.It.IsAny<GetAssetRelationshipsRequest>()))
                .ReturnsAsync(assetList);

            var request = new GetAssetRelationshipsRequest
            {
                SearchText = parentId,
            };

            // Act
            var result = await _sut.ExecuteAsync(request);

            // Assert
            result.Should().BeOfType<GetAssetRelationshipsResponse>();

            foreach (var asset in result.ChildAssets)
            {
                asset.ParentAssetIds.Should().Contain(parentId);
            }
        }
    }
}
