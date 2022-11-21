using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Controllers;
using HousingSearchApi.V1.UseCase.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.Controllers
{
    [Collection("LogCall collection")]
    public class GetProcessListControllerTests
    {
        private readonly Mock<IGetProcessListUseCase> _mockProcessListUseCase;
        private readonly GetProcessListController _classUnderTest;

        public GetProcessListControllerTests()
        {
            new LogCallAspectFixture().RunBeforeTests();

            _mockProcessListUseCase = new Mock<IGetProcessListUseCase>();
            _classUnderTest = new GetProcessListController(_mockProcessListUseCase.Object);
        }

        [Fact]
        public async Task GetProcessListShouldCallGetProcessListUseCase()
        {
            // given
            var request = new GetProcessListRequest();
            var response = new GetProcessListResponse();
            _mockProcessListUseCase.Setup(x => x.ExecuteAsync(request)).ReturnsAsync(response);

            // when
            await _classUnderTest.GetProcessList(request).ConfigureAwait(false);

            // then
            _mockProcessListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }
    }
}
