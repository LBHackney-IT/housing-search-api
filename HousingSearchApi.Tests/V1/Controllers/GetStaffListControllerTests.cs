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
    public class GetStaffListControllerTests
    {
        private readonly Mock<IGetStaffListUseCase> _mockGetStaffListUseCase;
        private readonly GetStaffListController _classUnderTest;


        public GetStaffListControllerTests()
        {
            new LogCallAspectFixture().RunBeforeTests();

            _mockGetStaffListUseCase = new Mock<IGetStaffListUseCase>();
            _classUnderTest = new GetStaffListController(_mockGetStaffListUseCase.Object);
        }

        [Fact]
        public async Task GetStaffListShouldCallGetStaffListUseCase()
        {
            // given
            var request = new GetStaffListRequest();
            var response = new GetStaffListResponse();
            _mockGetStaffListUseCase.Setup(x => x.ExecuteAsync(request)).ReturnsAsync(response);

            // when
            await _classUnderTest.GetStaffList(request).ConfigureAwait(false);

            // then
            _mockGetStaffListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }
    }
}
