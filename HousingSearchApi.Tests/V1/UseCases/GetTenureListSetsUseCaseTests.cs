using AutoFixture;
using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Gateways.Interfaces;
using HousingSearchApi.V1.UseCase;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.UseCases
{
    public class GetTenureListSetsUseCaseTests
    {
        private readonly GetTenureListSetsUseCase _classUnderTest;
        private readonly Mock<ISearchGateway> _mockSearchGateway;
        private readonly GetAllTenureListRequest _request;
        private readonly Fixture _fixture;

        public GetTenureListSetsUseCaseTests()
        {
            _mockSearchGateway = new Mock<ISearchGateway>();
            _classUnderTest = new GetTenureListSetsUseCase(_mockSearchGateway.Object);
            _fixture = new Fixture();

            _request = _fixture.Create<GetAllTenureListRequest>();
        }

        [Fact]
        public async Task CallsGetListOfTenuresSetsWithGetAllTenureListRequest()
        {
            _mockSearchGateway
                .Setup(x => x.GetListOfTenuresSets(_request))
                .ReturnsAsync(new GetAllTenureListResponse());

            _ = await _classUnderTest.ExecuteAsync(_request);

            _mockSearchGateway.Verify(x => x.GetListOfTenuresSets(_request), Times.Once);
        }

        [Fact]
        public async Task ReturnsGetAllTenureListResponseObject()
        {
            _mockSearchGateway
                .Setup(x => x.GetListOfTenuresSets(It.IsAny<GetAllTenureListRequest>()))
                .ReturnsAsync(new GetAllTenureListResponse());

            var response = await _classUnderTest.ExecuteAsync(_request);

            response.Should().BeOfType<GetAllTenureListResponse>();
        }
    }
}
