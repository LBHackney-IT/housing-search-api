using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Response;
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
    public class GetTenureListControllerTests
    {
        private readonly Mock<IGetAssetListUseCase> _mockGetAssetListUseCase;
        private readonly GetAssetListController _classUnderTest;


        public GetTenureListControllerTests()
        {
            new LogCallAspectFixture().RunBeforeTests();

            _mockGetAssetListUseCase = new Mock<IGetAssetListUseCase>();
            _classUnderTest = new GetAssetListController(_mockGetAssetListUseCase.Object);
        }

        [Fact]
        public async Task GetTenureListShouldCallGetTenureListUseCase()
        {
            // given
            var request = new HousingSearchRequest();
            var response = new GetAssetListResponse();
            _mockGetAssetListUseCase.Setup(x => x.ExecuteAsync(request)).ReturnsAsync(response);

            // when
            await _classUnderTest.GetAssetList(request).ConfigureAwait(false);

            // then
            _mockGetAssetListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }
    }
}
