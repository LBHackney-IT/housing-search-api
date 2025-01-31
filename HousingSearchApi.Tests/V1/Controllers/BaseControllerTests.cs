using FluentAssertions;
using Hackney.Core.Middleware;
using HousingSearchApi.V1.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using System.Collections.Generic;

namespace HousingSearchApi.Tests.V1.Controllers
{
    [TestFixture]
    public class BaseControllerTests
    {
        private BaseController _sut;
        private ControllerContext _controllerContext;
        private HttpContext _stubHttpContext;

        [SetUp]
        public void Init()
        {
            _stubHttpContext = new DefaultHttpContext();
            _controllerContext = new ControllerContext(new ActionContext(_stubHttpContext, new RouteData(), new ControllerActionDescriptor()));
            _sut = new BaseController();

            _sut.ControllerContext = _controllerContext;
        }

        [Test]
        public void GetCorrelationShouldThrowExceptionIfCorrelationHeaderUnavailable()
        {
            // Arrange + Act + Assert
            _sut.Invoking(x => x.GetCorrelationId())
                .Should().Throw<KeyNotFoundException>()
                .WithMessage("Request is missing a correlationId");
        }

        [Test]
        public void GetCorrelationShouldReturnCorrelationIdWhenExists()
        {
            // Arrange
            _stubHttpContext.Request.Headers.Append(HeaderConstants.CorrelationId, "123");

            // Act
            var result = _sut.GetCorrelationId();

            // Assert
            result.Should().BeEquivalentTo("123");
        }
    }
}
