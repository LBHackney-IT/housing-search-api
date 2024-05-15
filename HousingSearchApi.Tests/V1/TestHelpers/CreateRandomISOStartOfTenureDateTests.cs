using AutoFixture;
using System;
using System.Globalization;
using Xunit;
using QueryableTenure = Hackney.Shared.HousingSearch.Gateways.Models.Tenures.QueryableTenure;

namespace HousingSearchApi.Tests.V1.TestHelpers
{
    public class CreateRandomISOStartOfTenureDateTests
    {
        private readonly Fixture _fixture = new();

        //This is the date time format currently used in the index
        private readonly string _dateFormat = "yyyy-MM-ddTHH:mm:ssZ";
        private readonly CultureInfo _provider = CultureInfo.InvariantCulture;

        public CreateRandomISOStartOfTenureDateTests()
        {
            _fixture.Customize(new CreateRandomISOStartOfTenureDate());
        }

        [Fact]
        public void CreateRandomISOStartOfTenureDateReturnsDateInTeCorrectFormat()
        {
            // Arrange
            var result = _fixture.Create<QueryableTenure>();

            // Act + Assert
            //this will throw an exception and fail the test if the format is not correct
            DateTime.ParseExact(result.StartOfTenureDate, _dateFormat, _provider);
        }

        [Fact]
        public void CreateRandomISOStartOfTenureDateUsesCorrectTenureStartDateAddDaysMinValue()
        {
            // Act
            var result = CreateRandomISOStartOfTenureDate.TenureStartDateAddDaysMinValue;

            // Assert
            Assert.Equal(-3000, result);
        }

        [Fact]
        public void CreateRandomISOStartOfTenureDateUsesCorrectTenureStartDateAddDaysMaxValue()
        {
            // Act 
            var result = CreateRandomISOStartOfTenureDate.TenureStartDateAddDaysMaxValue;

            // Assert
            Assert.Equal(1, result);
        }
    }
}
