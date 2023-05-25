using AutoFixture;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Asset;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Controllers;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.Controllers
{
    [Collection("LogCall collection")]
    public class GetAssetRelationshipsControllerTests : ControllerTests
    {
        private readonly Mock<IGetAssetRelationshipsUseCase> _mockGetAssetRelationshipsUseCase;
        private readonly GetAssetRelationshipController _classUnderTest;
        private readonly Fixture _fixture = new();

        public GetAssetRelationshipsControllerTests()
        {
            new LogCallAspectFixture().RunBeforeTests();

            _mockGetAssetRelationshipsUseCase = new Mock<IGetAssetRelationshipsUseCase>();
            _classUnderTest = new GetAssetRelationshipController(_mockGetAssetRelationshipsUseCase.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetAssetRelationshipsReturns400WhenBadString(string searchText)
        {
            // Arrange
            var request = new GetAssetRelationshipsRequest()
            {
                SearchText = searchText
            };

            var useCaseResponse = new GetAssetRelationshipsResponse
            {
                ChildAssets = GenerateValidChildAssets(3)
            };
            _mockGetAssetRelationshipsUseCase
                .Setup(x => x.ExecuteAsync(request))
                .ReturnsAsync(useCaseResponse);

            // Act
            var response = await _classUnderTest
                .GetAssetRelationships(request)
                .ConfigureAwait(false);

            // Asset
            var statusCode = GetStatusCode(response);
            var message = GetResultData<string>(response);

            statusCode.Should().Be(400);
            message.Should().Be("Request searchtext cannot be blank");
        }

        [Fact]
        public async Task GetAssetRelationshipsReturnsDataAnd404WhenNoMatches()
        {
            // Arrange
            var request = new GetAssetRelationshipsRequest()
            {
                SearchText = Guid.NewGuid().ToString()
            };

            var useCaseResponse = new GetAssetRelationshipsResponse
            {
                ChildAssets = new List<Asset>()
            };
            _mockGetAssetRelationshipsUseCase
                .Setup(x => x.ExecuteAsync(request))
                .ReturnsAsync(useCaseResponse);

            // Act
            var response = await _classUnderTest
                .GetAssetRelationships(request)
                .ConfigureAwait(false);

            // Asset
            var statusCode = GetStatusCode(response);
            statusCode.Should().Be(204);
        }

        [Fact]
        public async Task GetAssetRelationshipsHappyPath()
        {
            // Arrange
            var request = new GetAssetRelationshipsRequest()
            {
                SearchText = Guid.NewGuid().ToString()
            };

            var useCaseResponse = new GetAssetRelationshipsResponse
            {
                ChildAssets = GenerateValidChildAssets(5)
            };
            _mockGetAssetRelationshipsUseCase
                .Setup(x => x.ExecuteAsync(request))
                .ReturnsAsync(useCaseResponse);

            // Act
            var response = await _classUnderTest
                .GetAssetRelationships(request)
                .ConfigureAwait(false);

            // Asset
            var statusCode = GetStatusCode(response);
            statusCode.Should().Be(200);

            var assets = GetResultData<GetAssetRelationshipsResponse>(response);
            assets.ChildAssets.Should().NotBeNull();
            assets.ChildAssets.Should().HaveCount(5);
        }

        private List<Asset> GenerateValidChildAssets(int numberOfAssets)
        {
            return _fixture.CreateMany<Asset>(numberOfAssets).ToList();
        }
    }
}
