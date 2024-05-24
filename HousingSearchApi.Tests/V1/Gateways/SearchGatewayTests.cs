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
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly GetAllTenureListRequest _getAllTenureListRequest;
        private readonly List<QueryableTenure> _queryableTenureList;

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

            _getAllTenureListRequest = _fixture.Create<GetAllTenureListRequest>();
            _queryableTenureList = _fixture.CreateMany<QueryableTenure>().ToList();
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
            SearchResponse<QueryableTenure> elasticSearchResponse = null;

            _elasticSearchWrapperMock
                .Setup(x => x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(_getAllTenureListRequest))
                .ReturnsAsync(elasticSearchResponse);

            var response = await _searchGateway.GetListOfTenuresSets(_getAllTenureListRequest);

            _elasticSearchWrapperMock
                .Verify(x =>
                    x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(_getAllTenureListRequest)
                    , Times.Once);
        }

        [Fact]
        public async Task GetListOfTenuresSets_WhenSearchResponseIsNullReturnsDefaultGetAllTenureListResponse()
        {
            SearchResponse<QueryableTenure> elasticSearchResponse = null;

            _elasticSearchWrapperMock
                .Setup(x => x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(_getAllTenureListRequest))
                .ReturnsAsync(elasticSearchResponse);

            var response = await _searchGateway.GetListOfTenuresSets(_getAllTenureListRequest);

            response.Should().BeOfType<GetAllTenureListResponse>();
            response.Should().BeEquivalentTo(new GetAllTenureListResponse());
        }

        [Fact]
        public async Task GetListOfTenuresSets_WhenSearchResponseHasTenuresAddsThemToResults()
        {
            var hitMock = new Mock<IHit<QueryableTenure>>();
            hitMock.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

            var hits = new List<IHit<QueryableTenure>>
            {
                hitMock.Object
            };

            var searchResponseMock = new Mock<ISearchResponse<QueryableTenure>>();
            searchResponseMock.Setup(x => x.Documents).Returns(_queryableTenureList);
            searchResponseMock.Setup(x => x.Hits).Returns(hits);

            _elasticSearchWrapperMock
                .Setup(x => x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(_getAllTenureListRequest))
                .ReturnsAsync(searchResponseMock.Object);

            var response = await _searchGateway.GetListOfTenuresSets(_getAllTenureListRequest);

            response.Tenures.Should().BeEquivalentTo(_queryableTenureList.Select(x => x.Create()));
        }

        [Fact]
        public async Task GetListOfTenuresSets_WhenSearchResponseHasTenuresSetsTheTotal()
        {
            var hitMock = new Mock<IHit<QueryableTenure>>();
            hitMock.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

            var hits = new List<IHit<QueryableTenure>>
            {
                hitMock.Object
            };

            var searchResponseMock = new Mock<ISearchResponse<QueryableTenure>>();
            searchResponseMock.Setup(x => x.Documents).Returns(_queryableTenureList);
            searchResponseMock.Setup(x => x.Total).Returns(_queryableTenureList.Count);
            searchResponseMock.Setup(x => x.Hits).Returns(hits);

            _elasticSearchWrapperMock
             .Setup(x => x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(_getAllTenureListRequest))
             .ReturnsAsync(searchResponseMock.Object);

            var response = await _searchGateway.GetListOfTenuresSets(_getAllTenureListRequest);

            response.Total().Should().Be(_queryableTenureList.Count);
        }

        [Fact]
        public async Task GetListOfTenuresSets_WhenSearchResponseHasTenuresSetsTheLastHitId()
        {
            //Record ids matching hits is not relevant here, just that the correct lastHitId is set
            var firstId = Guid.NewGuid().ToString();
            var firstHitMock = new Mock<IHit<QueryableTenure>>();
            firstHitMock.Setup(x => x.Id).Returns(firstId);

            var lastId = Guid.NewGuid().ToString();
            var lastHitMock = new Mock<IHit<QueryableTenure>>();
            lastHitMock.Setup(x => x.Id).Returns(lastId);

            var hits = new List<IHit<QueryableTenure>>
            {
                firstHitMock.Object,
                lastHitMock.Object
            };

            var searchResponseMock = new Mock<ISearchResponse<QueryableTenure>>();
            searchResponseMock.Setup(x => x.Documents).Returns(_queryableTenureList);
            searchResponseMock.Setup(x => x.Hits).Returns(hits);

            _elasticSearchWrapperMock
                .Setup(x => x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(_getAllTenureListRequest))
                .ReturnsAsync(searchResponseMock.Object);

            var response = await _searchGateway.GetListOfTenuresSets(_getAllTenureListRequest);

            response.LastHitId.Should().Be(lastId);
        }

        [Fact]
        public async Task GetListOfTenuresSets_WhenSearchResponseHasNoTenuresTheLastHitIdIsNull()
        {
            var searchResponseMock = new Mock<ISearchResponse<QueryableTenure>>();
            searchResponseMock.Setup(x => x.Documents).Returns(new List<QueryableTenure>());

            _elasticSearchWrapperMock
                .Setup(x => x.SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(_getAllTenureListRequest))
                .ReturnsAsync(searchResponseMock.Object);

            var response = await _searchGateway.GetListOfTenuresSets(_getAllTenureListRequest);

            response.LastHitId.Should().BeNull();
        }
    }
}
