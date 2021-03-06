using FluentAssertions;
using HousingSearchApi.V1.Gateways.Models.Assets;
using HousingSearchApi.V1.Gateways.Models.Persons;
using HousingSearchApi.V1.Gateways.Models.Tenures;
using HousingSearchApi.V1.Interfaces;
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
        public void GivenAnIndexSelectorWhenQueryableTenureShouldReturnTenuresIndex()
        {
            // Arrange + act
            var result = _sut.Create<HousingSearchApi.V1.Gateways.Models.Tenures.QueryableTenure>();

            // Assert
            result.Indices[0].Should().Be("tenures");
        }
    }
}
