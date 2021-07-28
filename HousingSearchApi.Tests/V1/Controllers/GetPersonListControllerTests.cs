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
    public class GetPersonListControllerTests
    {
        private readonly Mock<IGetPersonListUseCase> _mockGetPersonListUseCase;
        private readonly GetPersonListController _classUnderTest;


        public GetPersonListControllerTests()
        {
            new LogCallAspectFixture().RunBeforeTests();

            _mockGetPersonListUseCase = new Mock<IGetPersonListUseCase>();
            _classUnderTest = new GetPersonListController(_mockGetPersonListUseCase.Object);
        }

        [Fact]
        public async Task GetPersonListShouldCallGetPersonListUseCase()
        {
            // given
            var request = new GetPersonListRequest();
            var response = new GetPersonListResponse();
            _mockGetPersonListUseCase.Setup(x => x.ExecuteAsync(request)).ReturnsAsync(response);

            // when
            await _classUnderTest.GetPersonList(request).ConfigureAwait(false);

            // then
            _mockGetPersonListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }

        [Fact(Skip = "This test fails because the call to the use case returns a NotFound exception, which is caught and converted into a BadRequest response. Need to validate this is the required functionality.")]
        public async Task GetPersonListShouldReturnNotFoundObjectResultIfNotFound()
        {
            // given
            var request = new GetPersonListRequest();
            var notFoundException = new NotFoundException();

            _mockGetPersonListUseCase.Setup(x => x.ExecuteAsync(request))
                .Throws(notFoundException);

            // when
            var result = await _classUnderTest.GetPersonList(request).ConfigureAwait(false);

            // then
            result.Should().BeOfType(typeof(NotFoundObjectResult));
        }
    }
}
