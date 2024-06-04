using AutoFixture;
using FluentAssertions;
using Hackney.Core.Logging;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Controllers;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.Controllers
{
    [Collection("LogCall collection")]
    public class GetTenureListControllerTests
    {
        private readonly Mock<IGetTenureListUseCase> _mockGetTenureListUseCase;
        private readonly Mock<IGetTenureListSetsUseCase> _mockGetTenureListSetsUseCase;
        private readonly GetTenureListController _classUnderTest;
        private readonly GetAllTenureListRequest _getAllTenureListRequest;
        private readonly GetAllTenureListResponse _getAllTenureListResponse;
        private readonly Fixture _fixture;

        public GetTenureListControllerTests()
        {
            new LogCallAspectFixture().RunBeforeTests();

            _mockGetTenureListUseCase = new Mock<IGetTenureListUseCase>();
            _mockGetTenureListSetsUseCase = new Mock<IGetTenureListSetsUseCase>();
            _classUnderTest = new GetTenureListController(_mockGetTenureListUseCase.Object, _mockGetTenureListSetsUseCase.Object);

            _fixture = new Fixture();
            _getAllTenureListRequest = _fixture.Create<GetAllTenureListRequest>();
            _getAllTenureListResponse = _fixture.Create<GetAllTenureListResponse>();
        }

        [Fact]
        public async Task GetTenureListShouldCallGetTenureListUseCase()
        {
            // given
            var request = new GetTenureListRequest();
            var response = new GetTenureListResponse();
            _mockGetTenureListUseCase
                .Setup(x => x.ExecuteAsync(request))
                .ReturnsAsync(response);

            // when
            await _classUnderTest.GetTenureList(request).ConfigureAwait(false);

            // then
            _mockGetTenureListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }

        //all tenures
        [Fact]
        public async Task GetAllTenureList_CallsGetTenureListSetsUseCase()
        {
            // given
            _mockGetTenureListSetsUseCase
                .Setup(x => x.ExecuteAsync(It.IsAny<GetAllTenureListRequest>()))
                .ReturnsAsync(_getAllTenureListResponse);

            // when
            await _classUnderTest.GetAllTenureList(_getAllTenureListRequest).ConfigureAwait(false);

            //then
            _mockGetTenureListSetsUseCase.Verify(x => x.ExecuteAsync(It.IsAny<GetAllTenureListRequest>()), Times.Once);
        }

        [Fact]
        public async Task GetAllTenureList_CallsGetTenureListSetsUseCaseWithGivenGetAllTenureListRequest()
        {
            // given
            _mockGetTenureListSetsUseCase
                .Setup(x => x.ExecuteAsync(_getAllTenureListRequest))
                .ReturnsAsync(_getAllTenureListResponse);

            // when
            await _classUnderTest.GetAllTenureList(_getAllTenureListRequest).ConfigureAwait(false);

            // then
            _mockGetTenureListSetsUseCase.Verify(x => x.ExecuteAsync(_getAllTenureListRequest), Times.Once);
        }

        [Fact]
        public async Task GetAllTenureList_WhenUseCaseCallIsSuccessfulReturnsOkObjectResult()
        {
            // given
            _mockGetTenureListSetsUseCase
                .Setup(x => x.ExecuteAsync(It.IsAny<GetAllTenureListRequest>()))
                .ReturnsAsync(_getAllTenureListResponse);

            // when
            var result = await _classUnderTest.GetAllTenureList(_getAllTenureListRequest).ConfigureAwait(false);

            // then
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllTenureList_WhenUseCaseReturnsResultsIncludesThemInTheResponseObject()
        {
            // given
            _getAllTenureListResponse.LastHitId = null;
            _getAllTenureListResponse.LastHitTenureStartDate = null;

            _mockGetTenureListSetsUseCase
                .Setup(x => x.ExecuteAsync(It.IsAny<GetAllTenureListRequest>()))
                .ReturnsAsync(_getAllTenureListResponse);

            // when
            var result = await _classUnderTest.GetAllTenureList(_getAllTenureListRequest).ConfigureAwait(false);

            // then
            var expectedResults = new APIAllTenureResponse<GetAllTenureListResponse>()
            {
                Results = _getAllTenureListResponse
            };

            var expectedResponse = new OkObjectResult(expectedResults);

            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetAllTenureList_WhenUseCaseReturnsResultsAddsCorrectTotalToResponse()
        {
            // given
            var expectedTenuresCount = _getAllTenureListResponse.Tenures.Count;

            //this is normally set by the search gateway, so we can set it here
            _getAllTenureListResponse.SetTotal(expectedTenuresCount);

            _mockGetTenureListSetsUseCase
                .Setup(x => x.ExecuteAsync(It.IsAny<GetAllTenureListRequest>()))
                .ReturnsAsync(_getAllTenureListResponse);

            // when
            var result = await _classUnderTest.GetAllTenureList(_getAllTenureListRequest).ConfigureAwait(false);
            var okResult = result as OkObjectResult;
            var okResultvalue = (APIAllTenureResponse<GetAllTenureListResponse>) okResult.Value;

            // then
            okResultvalue.Total.Should().Be(expectedTenuresCount);
        }

        [Fact]
        public async Task GetAllTenureList_WhenUseCaseReturnsLastHitIdAddsItToResponse()
        {
            // given
            var expectedLastHitId = _getAllTenureListResponse.Tenures.Last().Id;

            _getAllTenureListResponse.LastHitId = expectedLastHitId;

            _mockGetTenureListSetsUseCase
                .Setup(x => x.ExecuteAsync(It.IsAny<GetAllTenureListRequest>()))
                .ReturnsAsync(_getAllTenureListResponse);

            // when
            var result = await _classUnderTest.GetAllTenureList(_getAllTenureListRequest).ConfigureAwait(false);
            var okResult = result as OkObjectResult;
            var okResultvalue = (APIAllTenureResponse<GetAllTenureListResponse>) okResult.Value;

            // then
            okResultvalue.LastHitId.Should().Be(expectedLastHitId);
        }

        [Fact]
        public async Task GetAllTenureList_WhenUseCaseReturnsLastHitTenureStartDateAddsItToTheResponse()
        {
            // given
            var expectedLastHitTenureStartDate = "12345678";

            _getAllTenureListResponse.LastHitTenureStartDate = expectedLastHitTenureStartDate;

            _mockGetTenureListSetsUseCase
                .Setup(x => x.ExecuteAsync(It.IsAny<GetAllTenureListRequest>()))
                .ReturnsAsync(_getAllTenureListResponse);

            // when
            var result = await _classUnderTest.GetAllTenureList(_getAllTenureListRequest).ConfigureAwait(false);
            var okResult = result as OkObjectResult;

            var okResultvalue = (APIAllTenureResponse<GetAllTenureListResponse>) okResult.Value;

            // then
            okResultvalue.LastHitTenureStartDate.Should().Be(expectedLastHitTenureStartDate);
        }

        [Fact]
        public async Task GetAllTenureList_ReturnsBadRequestObjectResultWhenUseCaseThrowsAnException()
        {
            // given
            var ex = new Exception();

            // when
            _mockGetTenureListSetsUseCase
                .Setup(x => x.ExecuteAsync(It.IsAny<GetAllTenureListRequest>()))
                .ThrowsAsync(ex);

            // then
            var result = await _classUnderTest.GetAllTenureList(_getAllTenureListRequest).ConfigureAwait(false);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetAllTenureList_ReturnsBadRequestObjectResultWithExceptionMessageWhenUseCaseThrowsAnException()
        {
            // given
            var exceptionMessage = "test exception message";
            var ex = new Exception(exceptionMessage);

            // when
            _mockGetTenureListSetsUseCase
                .Setup(x => x.ExecuteAsync(It.IsAny<GetAllTenureListRequest>()))
                .ThrowsAsync(ex);

            // then
            var result = await _classUnderTest.GetAllTenureList(_getAllTenureListRequest).ConfigureAwait(false);

            var badRequestObjectResult = result as BadRequestObjectResult;
            badRequestObjectResult.Value.Should().Be(exceptionMessage);
        }

        [Fact]
        public void GetAllTenureList_IsDecoratedWithCorrectAttributes()
        {
            // given
            var controllerType = _classUnderTest.GetType();
            var methodInfo = controllerType.GetMethod("GetAllTenureList");
            var methodAttributes = methodInfo.GetCustomAttributes(false);

            // then

            // [HttpGet, MapToApiVersion("1")]
            var apiVersionAttribute = methodAttributes
                .Where(x => x.GetType() == typeof(MapToApiVersionAttribute))
                .Select(x => x as MapToApiVersionAttribute).ToList();

            apiVersionAttribute.Count.Should().Be(1);
            apiVersionAttribute.First().Versions.First().MajorVersion.Should().Be(1);
            apiVersionAttribute.First().Versions.First().MinorVersion.Should().BeNull();

            // [LogCall(Microsoft.Extensions.Logging.LogLevel.Information)]
            var logCallAttributes = methodAttributes
                .Where(x => x.GetType() == typeof(LogCallAttribute)).Select(x => x as LogCallAttribute).ToList();

            logCallAttributes.Count.Should().Be(1);
            logCallAttributes.First().Level.Should().Be(Microsoft.Extensions.Logging.LogLevel.Information);

            // responses
            var producesResponseTypeAttributes = methodAttributes
                .Where(x => x.GetType() == typeof(ProducesResponseTypeAttribute))
                .Select(x => x as ProducesResponseTypeAttribute).ToList();

            producesResponseTypeAttributes.Count.Should().Be(2);

            // 200 response
            var okResponseAttribute = producesResponseTypeAttributes.First(x => x.StatusCode == 200).Type;
            var expectedOkResponseTypeFullName = typeof(APIAllTenureResponse<GetAllTenureListResponse>).FullName;
            okResponseAttribute.FullName.Should().Be(expectedOkResponseTypeFullName);

            // 400 response
            var badRequestObjectResultAttribute = producesResponseTypeAttributes.First(x => x.StatusCode == 400).Type;
            var expectedBadRequestObjectResultFullName = typeof(APIAllTenureResponse<BadRequestObjectResult>).FullName;
            badRequestObjectResultAttribute.FullName.Should().Be(expectedBadRequestObjectResultFullName);

            // route
            var routeAttributes = methodAttributes
                .Where(x => x.GetType() == typeof(RouteAttribute))
                .Select(x => x as RouteAttribute).ToList();

            routeAttributes.Count().Should().Be(1);
            routeAttributes.Any(x => x.Template.ToString() == "all").Should().BeTrue();
        }
    }
}

