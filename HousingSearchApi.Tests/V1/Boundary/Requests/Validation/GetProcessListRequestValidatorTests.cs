using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Requests.Validation;
using System;
using Xunit;
using FluentValidation.TestHelper;

namespace HousingSearchApi.Tests.V1.Boundary.Requests.Validation
{
    public class GetProcessListRequestValidatorTests
    {
        private readonly GetProcessListRequestValidator _sut;

        public GetProcessListRequestValidatorTests()
        {
            _sut = new GetProcessListRequestValidator();
        }

        [Fact]
        public void ShouldNotErrorWithBothTargetIdAndTargetType()
        {
            var query = new GetProcessListRequest()
            {
                SearchText = "abc",
                TargetId = Guid.NewGuid(),
                TargetType = "tenure"
            };
            var result = _sut.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldNotErrorWithOnlySearchText()
        {
            var query = new GetProcessListRequest()
            {
                SearchText = "abc",
            };
            var result = _sut.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldNotErrorOnlyTargetIddSearchText()
        {
            var query = new GetProcessListRequest()
            {
                SearchText = "abc",
                TargetId = Guid.NewGuid()
            };
            var result = _sut.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldErrorWithOnlyTargetType()
        {
            var query = new GetProcessListRequest()
            {
                SearchText = "abc",
                TargetType = "tenure"
            };
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.TargetId);
        }



    }
}
