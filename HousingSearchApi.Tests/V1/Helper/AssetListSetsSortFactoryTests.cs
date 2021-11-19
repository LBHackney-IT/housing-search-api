using FluentAssertions;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var result = _sut.Create<QueryableAsset>(new HousingSearchRequest());

            // Assert
            result.Should().BeOfType(typeof(DefaultSort<QueryableAsset>));
        }

        [Fact]
        public void ShouldReturnAssetIdAscWhenRequestAssetIdAndAsc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableAsset>(new HousingSearchRequest { SortBy = "assetId", IsDesc = false });

            // Assert
            result.Should().BeOfType(typeof(AssetIdAsc));
        }

        [Fact]
        public void ShouldReturnAssetIdDescWhenRequestAssetIdAndDesc()
        {
            // Arrange + Act
            var result = _sut.Create<QueryableAsset>(new HousingSearchRequest { SortBy = "assetId", IsDesc = true });

            // Assert
            result.Should().BeOfType(typeof(AssetIdDesc));
        }
    }
}
