using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Boundary.Responses.Transactions;
using HousingSearchApi.V1.Controllers;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.Controllers
{
    [Collection("LogCall collection")]
    public class GetTransactionListControllerTests
    {
        private readonly Mock<IGetTransactionListUseCase> _mockGetTransactionListUseCase;
        private readonly GetTransactionListController _classUnderTest;

        public GetTransactionListControllerTests()
        {
            new LogCallAspectFixture().RunBeforeTests();

            _mockGetTransactionListUseCase = new Mock<IGetTransactionListUseCase>();
            _classUnderTest = new GetTransactionListController(_mockGetTransactionListUseCase.Object);
        }

        [Fact]
        public async Task GetTransactionListReturnsOk()
        {
            const int transactionsCount = 10;
            var request = new GetTransactionListRequest();
            var response = GetTransactionListResponse.Create(transactionsCount, new List<TransactionResponse>());

            _mockGetTransactionListUseCase
                .Setup(x => x.ExecuteAsync(request))
                .ReturnsAsync(response);

            var controllerResponse = await _classUnderTest.GetTransactionList(request).ConfigureAwait(false);

            _mockGetTransactionListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);

            var okObjectResult = controllerResponse as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.Should().NotBeNull();

            var apiResponse = okObjectResult.Value as APIResponse<GetTransactionListResponse>;
            apiResponse.Should().NotBeNull();

            apiResponse.Results.Should().BeEquivalentTo(response);
            apiResponse.Total.Should().Be(transactionsCount);
        }

        [Fact]
        public async Task GetTransactionListThrowsIfUseCaseReturnsNull()
        {
            var request = new GetTransactionListRequest();

            _mockGetTransactionListUseCase
                .Setup(x => x.ExecuteAsync(request))
                .Throws(new Exception("Some error"));

            var controllerResponse = await _classUnderTest.GetTransactionList(request).ConfigureAwait(false);

            _mockGetTransactionListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);

            var objectResult = controllerResponse as BadRequestObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.Value.Should().Be("Some error");
        }
    }
}
