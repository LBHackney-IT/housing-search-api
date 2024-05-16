using FluentAssertions;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using System;
using System.Globalization;
using Xunit;

namespace HousingSearchApi.Tests.V1.TestHelpers
{
    public class QueryableTenureHelperTests
    {
        private readonly CultureInfo _provider = CultureInfo.InvariantCulture;
        private static readonly string _startOfTenureDateFormat = "yyyy-MM-ddTHH:mm:ssZ";
        private readonly QueryableTenure _queryableTenure;

        //provide coverage for QueryableTenure properties that require custom handling 
        public QueryableTenureHelperTests()
        {
            _queryableTenure = QueryableTenureHelper.CreateQueyableTenure();
        }

        [Fact]
        public void CreateQueryableTenureReturnsQueryableTenure()
        {
            _queryableTenure.Should().NotBeNull();
            _queryableTenure.Should().BeOfType(typeof(QueryableTenure));
        }

        [Fact]
        public void CreateQueryableTenureSetsStartOfTenureDateInTheCorrectFormat()
        {
            //this will throw an exception and fail the test if the format is not correct
            DateTime.ParseExact(_queryableTenure.StartOfTenureDate, _startOfTenureDateFormat, _provider);
        }

        [Fact]
        public void CreateQueryableTenureSetsStartOfTenureDateToBeInThePast()
        {
            //date being in the past is not that relevant for the current test setups, but it makes more sense looking at the business logic for tenures
            DateTime.ParseExact(_queryableTenure.StartOfTenureDate, _startOfTenureDateFormat, _provider).Should().BeBefore(DateTime.Now);
        }
    }
}
