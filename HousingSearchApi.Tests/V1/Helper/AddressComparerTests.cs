using Bogus;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Asset;
using HousingSearchApi.V1.Helper;
using System;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class AddressComparerTests
    {
        private readonly AddressComparer _classUnderTest;

        public AddressComparerTests()
        {
            _classUnderTest = new AddressComparer();
        }


        [Fact]
        public void Compare_DoesntThrowException_WhenAddressLine1ContainsNoSpaces()
        {
            // Arrange
            var address1 = new AssetAddress
            {
                AddressLine1 = "71A",
                AddressLine2 = "GREENWOOD ROAD"
            };

            var address2 = new AssetAddress
            {
                AddressLine1 = "1 Pitcairn House"
            };


            // Act
            Func<int> func = () => _classUnderTest.Compare(address1, address2);

            // Assert
            func.Should().NotThrow();
        }
    }
}
