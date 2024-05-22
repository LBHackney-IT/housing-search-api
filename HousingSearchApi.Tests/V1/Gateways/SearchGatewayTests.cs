using AutoFixture;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Asset;
using Hackney.Shared.HousingSearch.Gateways.Models.Assets;
using Hackney.Shared.HousingSearch.Gateways.Models.Tenures;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Factories;
using HousingSearchApi.V1.Gateways;
using HousingSearchApi.V1.Helper.Interfaces;
using HousingSearchApi.V1.Interfaces;
using Moq;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HousingSearchApi.Tests.V1.Gateways
{
    [Collection("LogCall collection")]
    public class SearchGatewayTests
    {
        private readonly SearchGateway _searchGateway;
        private readonly Mock<IElasticSearchWrapper> _elasticSearchWrapperMock;
        private readonly Mock<ICustomAddressSorter> _customAddressSorterMock;

        private readonly Fixture _fixture = new Fixture();

        public SearchGatewayTests()
        {
            new LogCallAspectFixture().RunBeforeTests();

            _elasticSearchWrapperMock = new Mock<IElasticSearchWrapper>();
            _customAddressSorterMock = new Mock<ICustomAddressSorter>();

            _searchGateway = new SearchGateway(
                _elasticSearchWrapperMock.Object,
                _customAddressSorterMock.Object
            );
        }

        [Fact]
        public async Task GetListOfAssets_WhenCustomSortingFalse_DoesntUseCustomSort()
        {
            // Arrange
            var query = new GetAssetListRequest
            {
                UseCustomSorting = false
            };

            var elasticSearchResponse = _fixture.Create<SearchResponse<QueryableAsset>>();

            _elasticSearchWrapperMock
                .Setup(x => x.Search<QueryableAsset, GetAssetListRequest>(query))
                .ReturnsAsync(elasticSearchResponse);

            // Act
            var response = await _searchGateway.GetListOfAssets(query);

            // Assert
            response.Should().BeEquivalentTo(elasticSearchResponse.ToResponse());

            _customAddressSorterMock
                .Verify(x => x.FilterResponse(query, It.IsAny<GetAssetListResponse>()), Times.Never);
        }

        [Fact]
        public async Task GetListOfAssets_WhenCustomSortingTrue_OverridesPageSize()
        {
            // Arrange
            var query = new GetAssetListRequest
            {
                UseCustomSorting = true
            };

            var elasticSearchResponse = _fixture.Create<SearchResponse<QueryableAsset>>();

            _elasticSearchWrapperMock
                .Setup(x => x.Search<QueryableAsset, GetAssetListRequest>(query))
                .ReturnsAsync(elasticSearchResponse);

            // Act
            var response = await _searchGateway.GetListOfAssets(query);

            // Assert
            query.PageSize.Should().Be(650);
        }

        [Fact]
        public async Task GetListOfAssets_WhenCustomSortingTrue_CallsCustomAddressSorter()
        {
            // Arrange
            var query = new GetAssetListRequest
            {
                UseCustomSorting = true
            };

            var elasticSearchResponse = _fixture.Create<SearchResponse<QueryableAsset>>();

            _elasticSearchWrapperMock
                .Setup(x => x.Search<QueryableAsset, GetAssetListRequest>(query))
                .ReturnsAsync(elasticSearchResponse);

            // Act
            var response = await _searchGateway.GetListOfAssets(query);

            // Assert
            response.Should().BeEquivalentTo(elasticSearchResponse.ToResponse());

            _customAddressSorterMock
                .Verify(x => x.FilterResponse(query, It.IsAny<GetAssetListResponse>()), Times.Once);
        }

        [Fact]
        public async Task GetChildAssets_WhenSearchResponseIsNullReturnEmptyList()
        {
            // Arrange
            var query = new GetAssetRelationshipsRequest
            {
                SearchText = "test"
            };

            SearchResponse<QueryableAsset> elasticSearchResponse = null;

            _elasticSearchWrapperMock
                .Setup(x => x.Search<QueryableAsset, GetAssetRelationshipsRequest>(query))
                .ReturnsAsync(elasticSearchResponse);

            // Act
            var response = await _searchGateway.GetChildAssets(query);

            // Assert
            response.Should().BeEquivalentTo(new List<Asset>());
        }

        //tenures
        [Fact]
        public async Task GetListOfTenuresSets_CallsSearchTenuresSets()
        {
            var query = new GetAllTenureListRequest();

            SearchResponse<QueryableTenure> elasticSearchResponse = null;

            _elasticSearchWrapperMock
                .Setup(x => x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(query))
                .ReturnsAsync(elasticSearchResponse);

            var response = await _searchGateway.GetListOfTenuresSets(query);

            _elasticSearchWrapperMock
                .Verify(x =>
                    x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(It.IsAny<GetAllTenureListRequest>())
                    , Times.Once);
        }

        [Fact]
        public async Task GetListOfTenuresSets_CallsSearchTenuresSetsWithCorrectQuery()
        {
            var query = _fixture.Create<GetAllTenureListRequest>();

            SearchResponse<QueryableTenure> elasticSearchResponse = null;

            _elasticSearchWrapperMock
                .Setup(x => x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(query))
                .ReturnsAsync(elasticSearchResponse);

            var response = await _searchGateway.GetListOfTenuresSets(query);

            _elasticSearchWrapperMock
                .Verify(x =>
                    x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(query)
                    , Times.Once);
        }

        [Fact]
        public async Task GetListOfTenuresSets_WhenSearchResponseIsNullReturnsCorrectGetAllTenureListResponse()
        {
            var query = _fixture.Create<GetAllTenureListRequest>();
            SearchResponse<QueryableTenure> elasticSearchResponse = null;

            _elasticSearchWrapperMock
                .Setup(x => x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(query))
                .ReturnsAsync(elasticSearchResponse);

            var response = await _searchGateway.GetListOfTenuresSets(query);

            response.Should().BeOfType<GetAllTenureListResponse>();
            response.LastHitId.Should().BeNull();
            response.Tenures.Count.Should().Be(0);
        }
    }
}
