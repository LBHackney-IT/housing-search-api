using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoFixture;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Accounts;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Controllers;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HousingSearchApi.Tests.V1.Controllers
{
    public class GetAccountListControllerTests
    {
        private readonly Fixture _fixture;
        private readonly GetAccountListController _sut;
        private readonly Mock<IGetAccountListUseCase> _getAccountListUseCase;

        public GetAccountListControllerTests()
        {
            new LogCallAspectFixture().RunBeforeTests();

            _fixture = new Fixture();
            _getAccountListUseCase = new Mock<IGetAccountListUseCase>();
            _sut = new GetAccountListController(_getAccountListUseCase.Object);
        }

        [Fact]
        public void GetAccountListWithValidInputReturnsOk()
        {
            GetAccountListRequest request = _fixture.Create<GetAccountListRequest>();
            List<Account> accounts = _fixture.Create<List<Account>>();

            GetAccountListResponse response = GetAccountListResponse.Create(accounts);
            response.SetTotal(accounts.Count);

            _getAccountListUseCase.Setup(p =>
                    p.ExecuteAsync(It.IsAny<GetAccountListRequest>()))
                .ReturnsAsync(response);

            var resultObject = _sut.GetAccountList(request);
            resultObject.Should().NotBeNull();
            var okResult = resultObject.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be((int) HttpStatusCode.OK);
            var apiResponse = okResult.Value;
            apiResponse.Should().BeOfType<APIResponse<GetAccountListResponse>>();
            ((APIResponse<GetAccountListResponse>) apiResponse).Results.Accounts.ForEach(p => p.Should().NotBeNull());
            ((APIResponse<GetAccountListResponse>) apiResponse).Total.Should().Be(accounts.Count);
        }

        [Fact]
        public void GetAccountListWithNullInputReturnsBadRequest()
        {
            GetAccountListRequest request = _fixture.Create<GetAccountListRequest>();
            List<Account> accounts = _fixture.Create<List<Account>>();

            GetAccountListResponse response = GetAccountListResponse.Create(accounts);
            response.SetTotal(accounts.Count);

            _getAccountListUseCase.Setup(p =>
                    p.ExecuteAsync(It.IsAny<GetAccountListRequest>()))
                .ReturnsAsync(It.IsAny<GetAccountListResponse>());

            var resultObject = _sut.GetAccountList(null);
            resultObject.Should().NotBeNull();
            resultObject.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GetAccountListWithInternalExceptionReturnsError500()
        {
            GetAccountListRequest request = _fixture.Create<GetAccountListRequest>();
            List<Account> accounts = _fixture.Create<List<Account>>();

            GetAccountListResponse response = GetAccountListResponse.Create(accounts);
            response.SetTotal(accounts.Count);

            _getAccountListUseCase.Setup(p =>
                    p.ExecuteAsync(It.IsAny<GetAccountListRequest>()))
                .Throws<Exception>();

            var resultObject = _sut.GetAccountList(request);
            resultObject.Should().NotBeNull();
            ((ObjectResult) resultObject.Result).StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}
