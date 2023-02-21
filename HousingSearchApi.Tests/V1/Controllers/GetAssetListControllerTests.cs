using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Controllers;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.Controllers
{
    [Collection("LogCall collection")]
    public class GetAssetListControllerTests : ControllerTests
    {
        private readonly Mock<IGetAssetListUseCase> _mockGetAssetListUseCase;
        private readonly Mock<IGetAssetListSetsUseCase> _mockGetAssetListSetsUseCase;
        private readonly GetAssetListController _classUnderTest;

        public GetAssetListControllerTests()
        {
            new LogCallAspectFixture().RunBeforeTests();

            _mockGetAssetListUseCase = new Mock<IGetAssetListUseCase>();
            _mockGetAssetListSetsUseCase = new Mock<IGetAssetListSetsUseCase>();
            _classUnderTest = new GetAssetListController(_mockGetAssetListUseCase.Object, _mockGetAssetListSetsUseCase.Object);
        }

        [Fact]
        public async Task GetAssetListShouldCallGetAssetListUseCase()
        {
            // given
            var request = new GetAssetListRequest();
            var response = new GetAssetListResponse();
            _mockGetAssetListUseCase.Setup(x => x.ExecuteAsync(request)).ReturnsAsync(response);

            // when
            await _classUnderTest.GetAssetList(request).ConfigureAwait(false);

            // then
            _mockGetAssetListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }
        [Fact]
        public async Task GetAssetListSetsShouldCallGetAssetListSetsUseCase()
        {
            // given
            var request = new GetAllAssetListRequest();
            var response = new GetAllAssetListResponse();
            _mockGetAssetListSetsUseCase.Setup(x => x.ExecuteAsync(request)).ReturnsAsync(response);

            // when
            await _classUnderTest.GetAllAssetList(request).ConfigureAwait(false);

            // then
            _mockGetAssetListSetsUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }

        [Fact]
        public async Task GetAssetListSetsShouldFilterOnMultplesUseCase()
        {
            // given
            var request = new GetAllAssetListRequest()
            {
                NumberOfBedSpaces = "2",
                NumberOfCots = "2"
            };
            var response = new GetAllAssetListResponse();
            _mockGetAssetListSetsUseCase.Setup(x => x.ExecuteAsync(request)).ReturnsAsync(response);

            // when
            await _classUnderTest.GetAllAssetList(request).ConfigureAwait(false);

            // then
            _mockGetAssetListSetsUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }

        [Fact]
        public async Task GetAssetListSetsShouldFilterOnMultplesWithSearchTextUseCase()
        {
            // given
            var request = new GetAllAssetListRequest()
            {
                NumberOfBedSpaces = "2",
                NumberOfCots = "2",
                SearchText = "test"
            };
            var response = new GetAllAssetListResponse();
            _mockGetAssetListSetsUseCase.Setup(x => x.ExecuteAsync(request)).ReturnsAsync(response);

            // when
            await _classUnderTest.GetAllAssetList(request).ConfigureAwait(false);

            // then
            _mockGetAssetListSetsUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }

        [Fact]
        public async Task GetAssetReturnsBadRequestWhenUseCustomSortingTrueAndOtherSearchParametersIncluded()
        {
            // given
            var request = new GetAssetListRequest()
            {
                UseCustomSorting = true,
                Page = 1,
                SortBy = "Something"
            };

            var useCaseResponse = new GetAssetListResponse();

            _mockGetAssetListUseCase
                .Setup(x => x.ExecuteAsync(request))
                .ReturnsAsync(useCaseResponse);

            // when
            var response = await _classUnderTest
                .GetAssetList(request)
                .ConfigureAwait(false);

            // then
            var statusCode = GetStatusCode(response);

            statusCode.Should().Be(400);
        }
    }
}
