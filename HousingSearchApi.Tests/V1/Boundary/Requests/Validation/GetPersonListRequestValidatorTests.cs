using FluentValidation.TestHelper;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Requests.Validation;
using Xunit;

namespace HousingSearchApi.Tests.V1.Boundary.Request.Validation
{
    public class GetPersonListRequestValidatorTests
    {
        private readonly GetPersonListRequestValidator _sut;

        public GetPersonListRequestValidatorTests()
        {
            _sut = new GetPersonListRequestValidator();
        }

        private static GetPersonListRequest CreateValidRequest()
        {
            return new GetPersonListRequest()
            {
                SearchText = "Some search text"
            };
        }

        [Fact]
        public void ValidRequestShouldNotError()
        {
            var query = CreateValidRequest();
            var result = _sut.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("Something <sometag>")]
        public void RequestShouldErrorWithInvalidSearchText(string invalidSearchText)
        {
            var query = CreateValidRequest();
            query.SearchText = invalidSearchText;
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.SearchText);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void RequestShouldErrorWithInvalidPageSize(int invalidPageSize)
        {
            var query = CreateValidRequest();
            query.PageSize = invalidPageSize;
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }

        [Fact]
        public void RequestShouldErrorWithInvalidSortBy()
        {
            var query = CreateValidRequest();
            query.SortBy = "something<sometag>";
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.SortBy);
        }
    }
}
