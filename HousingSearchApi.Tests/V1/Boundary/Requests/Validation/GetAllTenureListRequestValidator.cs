using FluentAssertions;
using FluentValidation.TestHelper;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Requests.Validation;
using System;
using System.Linq;
using Xunit;

namespace HousingSearchApi.Tests.V1.Boundary.Requests.Validation
{
    public class GetAllTenureListRequestValidatorTests
    {
        private readonly GetAllTenureListRequestValidator _classUnderTest;

        public GetAllTenureListRequestValidatorTests()
        {
            _classUnderTest = new GetAllTenureListRequestValidator();
        }

        [Fact]
        public void ShouldErrorWhenLastHitTenureStartDateIsNotInMillisecondsFormat()
        {
            var query = new GetAllTenureListRequest()
            {
                LastHitTenureStartDate = DateTime.Now.ToString()
            };

            var result = _classUnderTest.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.LastHitTenureStartDate);
        }

        [Fact]
        public void ShouldErrorWhenLastHitTenureStartDateStringLooksDangerous()
        {
            var query = new GetAllTenureListRequest()
            {
                LastHitTenureStartDate = "<string"
            };

            var result = _classUnderTest.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.LastHitTenureStartDate);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldErrorWhenLastHitTenureStartDateIsProvidedButLastHitIdIsNullOrEmpty(string lastHitId)
        {
            var query = new GetAllTenureListRequest()
            {
                LastHitTenureStartDate = "1234567",
                LastHitId = lastHitId
            };

            var expectedErrorMessage = "LastHitId must be provided with LastHitTenureStartDate";

            var result = _classUnderTest.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.LastHitId);
            result.Errors.Single().ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Fact]
        public void ShouldNotErrorWhenLastHitTenureStartDateIsInMillisecondsFormat()
        {
            DateTime dt = DateTime.Now.AddDays(-10);
            var startDateInMillisecondsSinceEpoch = (long) dt.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

            var query = new GetAllTenureListRequest()
            {
                LastHitTenureStartDate = startDateInMillisecondsSinceEpoch.ToString()
            };

            var result = _classUnderTest.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(x => x.LastHitTenureStartDate);
        }

        [Fact]
        public void ShoulNotErrorWhenLastHitTenureStartDateIsNotProvided()
        {
            var query = new GetAllTenureListRequest();

            var result = _classUnderTest.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(x => x.LastHitTenureStartDate);
        }

    }
}
