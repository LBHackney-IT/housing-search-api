using AutoFixture;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Asset;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Helper;
using Moq;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class CustomAddressSorterTests
    {
        private readonly CustomAddressSorter _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        public CustomAddressSorterTests()
        {
            var comparer = new AddressComparer();

            _classUnderTest = new CustomAddressSorter(comparer);
        }

        [Fact]
        public void FilterResponse_WhenCalled_IncludesAddressesWithMatchingUPRN()
        {
            // Arrange
            var asset = _fixture.Create<Asset>();

            var content = new GetAssetListResponse {
                Assets = new List<Asset> { asset }
            };

            var searchModel = new GetAssetListRequest {
                SearchText = asset.AssetId,
            };

            // Act
            _classUnderTest.FilterResponse(searchModel, content);

            // Assert

            content.Assets.Should().HaveCount(1);
        }
    }
}
