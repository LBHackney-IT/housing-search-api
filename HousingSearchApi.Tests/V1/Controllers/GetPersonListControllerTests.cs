using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation.Results;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Controllers;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace HousingSearchApi.Tests.V1.Controllers
{
    public class GetPersonListControllerTests
    {
        private Mock<IGetPersonListUseCase> _mockGetPersonListUseCase;
        private GetPersonListController _classUnderTest;

        [SetUp]
        public void Init()
        {
            _mockGetPersonListUseCase = new Mock<IGetPersonListUseCase>();
            _classUnderTest = new GetPersonListController(_mockGetPersonListUseCase.Object);
        }

        [Test]
        public async Task GetPersonListShouldCallGetPersonListUseCase()
        {
            // given
            var request = new GetPersonListRequest();

            // when
            await _classUnderTest.GetPersonList(request);

            // then
            _mockGetPersonListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }

        [Test]
        public async Task GetPersonListShouldReturnNotFoundObjectResultIfNotFound()
        {
            // given
            var request = new GetPersonListRequest();
            var notFoundException = new NotFoundException();

            _mockGetPersonListUseCase.Setup(x => x.ExecuteAsync(request))
                .Throws(notFoundException);

            // when
            var result = await _classUnderTest.GetPersonList(request);

            // then
            result.Should().BeOfType(typeof(NotFoundObjectResult));
        }
    }
}
