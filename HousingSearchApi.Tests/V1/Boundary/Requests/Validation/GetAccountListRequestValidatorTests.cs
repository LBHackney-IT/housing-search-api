using System;
using FluentValidation.TestHelper;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Requests.Validation;
using HousingSearchApi.V1.Helper;
using Xunit;

namespace HousingSearchApi.Tests.V1.Boundary.Requests.Validation
{
    public class GetAccountListRequestValidatorTests
    {
        private readonly GetAccountListRequestValidator _sut;
        public GetAccountListRequestValidatorTests()
        {
            _sut = new GetAccountListRequestValidator();
        }

        [Theory]
        [MemberData(nameof(MockParametersForValidator.GetTestData), MemberType = typeof(MockParametersForValidator))]
        private void ValidatorSHoudNotFireException(string searchText, Guid targetId)
        {
            GetAccountListRequest request = new GetAccountListRequest
            {
                SearchText = searchText,
                TargetId = targetId
            };
            var result = _sut.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        private void ValidatorSHoudFireException()
        {
            GetAccountListRequest request = new GetAccountListRequest
            {
                SearchText = null,
                TargetId = Guid.Empty
            };
            var result = _sut.TestValidate(request);
            result.ShouldHaveAnyValidationError();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void RequestShouldErrorWithInvalidPageSize(int invalidPageSize)
        {
            GetAccountListRequest request = new GetAccountListRequest
            {
                SearchText = "Some thing to search",
                TargetId = Guid.Empty,
                PageSize = invalidPageSize
            };
            var result = _sut.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }

        [Theory]
        [InlineData(-10)]
        public void RequestShouldErrorWithInvalidPageNumber(int invalidPageNumber)
        {
            GetAccountListRequest request = new GetAccountListRequest
            {
                SearchText = "Some thing to search",
                Page = invalidPageNumber
            };
            var result = _sut.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Page);
        }

        [Fact]
        public void RequestShouldErrorWithInvalidSortBy()
        {
            GetAccountListRequest request = new GetAccountListRequest
            {
                SearchText = "Some thing to search",
                TargetId = Guid.Empty,
                SortBy = "something<sometag>"
            };
            var result = _sut.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.SortBy);
        }



    }
}
