using FluentAssertions;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Sorting;
using HousingSearchApi.V1.Infrastructure.Sorting;
using Xunit;

namespace HousingSearchApi.Tests.V1.Helper
{
    public class AssetListSetsSortFactoryTests
    {
        private SortFactory _sut;

        public AssetListSetsSortFactoryTests()
        {
            _sut = new SortFactory();
        }
        [Fact]
        public void ShouldNotSortAsDefault()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableAsset, GetAssetListRequest>(new GetAssetListRequest());

            // Assert
            result.Should().BeOfType(typeof(DefaultSort<QueryableAsset>));
        }

        [Fact]
        public void ShouldReturnAssetIdAscWhenRequestAssetIdAndAsc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableAsset, GetAssetListRequest>(new GetAssetListRequest { SortBy = "assetId", IsDesc = false });

            // Assert
            result.Should().BeOfType(typeof(AssetIdAsc));
        }

        [Fact]
        public void ShouldReturnAssetIdDescWhenRequestAssetIdAndDesc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableAsset, GetAssetListRequest>(new GetAssetListRequest { SortBy = "assetId", IsDesc = true });

            // Assert
            result.Should().BeOfType(typeof(AssetIdDesc));
        }
    }
}
