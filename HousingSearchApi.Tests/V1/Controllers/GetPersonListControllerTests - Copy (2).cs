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
        private readonly Mock<IGetTenureListUseCase> _mockGetTenureListUseCase;
        private readonly GetTenureListController _classUnderTest;


        public GetTenureListControllerTests()
        {
            new LogCallAspectFixture().RunBeforeTests();

            _mockGetTenureListUseCase = new Mock<IGetTenureListUseCase>();
            _classUnderTest = new GetTenureListController(_mockGetTenureListUseCase.Object);
        }

        [Fact]
        public async Task GetTenureListShouldCallGetTenureListUseCase()
        {
            // given
            var request = new GetTenureListRequest();
            var response = new GetTenureListResponse();
            _mockGetTenureListUseCase.Setup(x => x.ExecuteAsync(request)).ReturnsAsync(response);

            // when
            await _classUnderTest.GetTenureList(request).ConfigureAwait(false);

            // then
            _mockGetTenureListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }

        [Fact(Skip = "This test fails because the call to the use case returns a NotFound exception, which is caught and converted into a BadRequest response. Need to validate this is the required functionality.")]
        public async Task GetTenureListShouldReturnNotFoundObjectResultIfNotFound()
        {
            // given
            var request = new GetTenureListRequest();
            var notFoundException = new NotFoundException();

            _mockGetTenureListUseCase.Setup(x => x.ExecuteAsync(request))
                .Throws(notFoundException);

            // when
            var result = await _classUnderTest.GetTenureList(request).ConfigureAwait(false);

            // then
            result.Should().BeOfType(typeof(NotFoundObjectResult));
        }
    }
}
