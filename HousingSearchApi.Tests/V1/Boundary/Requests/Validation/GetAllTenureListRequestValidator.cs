using FluentValidation.TestHelper;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Requests.Validation;
using System;
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
