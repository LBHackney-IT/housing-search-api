using FluentAssertions;
using HousingSearchApi.V1.Gateways.Models;
using HousingSearchApi.V1.Interfaces;
using NUnit.Framework;
using Xunit;

namespace HousingSearchApi.Tests.V1.Interfaces
{
    public class IndexSelectorTests
    {
        private IndexSelector _sut;

        public IndexSelectorTests()
        {
            _sut = new IndexSelector();
        }

        [Fact]
        public void GivenAnIndexSelectorWhenQueryablePersonShouldReturnPersonsIndex()
        {
            // Arrange + act
            var result = _sut.Create<QueryablePerson>();

            // Assert
            result.Indices[0].Should().Be("persons");
        }

        [Fact]
        public void GivenAnIndexSelectorWhenQueryableAssetShouldReturnAssetsIndex()
        {
            // Arrange + act
            var result = _sut.Create<QueryableAsset>();

            // Assert
            result.Indices[0].Should().Be("assets");
        }

        [Fact]
        public void GivenAnIndexSelectorWhenQueryableTenurenShouldReturnTenuresIndex()
        {
            // Arrange + act
            var result = _sut.Create<QueryableTenure>();

            // Assert
            result.Indices[0].Should().Be("tenures");
        }
    }
}
