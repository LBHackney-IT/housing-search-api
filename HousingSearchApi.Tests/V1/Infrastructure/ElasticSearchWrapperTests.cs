using AutoFixture;
using Elasticsearch.Net;
using FluentAssertions;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Infrastructure;
using HousingSearchApi.V1.Infrastructure.Sorting;
using HousingSearchApi.V1.Interfaces;
using HousingSearchApi.V1.Interfaces.Factories;
using HousingSearchApi.V1.Interfaces.Filtering;
using HousingSearchApi.V1.Interfaces.Sorting;
using Microsoft.Extensions.Logging;
using Moq;
using Nest;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using QueryableTenure = Hackney.Shared.HousingSearch.Gateways.Models.Tenures.QueryableTenure;

namespace HousingSearchApi.Tests.V1.Infrastructure
{
    public class ElasticSearchWrapperTests
    {
        private readonly Mock<IElasticClient> _elasticClientMock;
        private readonly Mock<IQueryFactory> _queryFactoryMock;
        private readonly Mock<IPagingHelper> _pagingHelperMock;
        private readonly Mock<ISortFactory> _sortfactoryMock;
        private readonly Mock<IFilterFactory> _filterFactoryMock;
        private readonly Mock<IIndexSelector> _indexSelectorMock;
        private readonly Mock<ILogger<ElasticSearchWrapper>> _loggerMock;
        private readonly ElasticSearchWrapper _elasticSearchWrapper;
        private readonly GetAllTenureListRequest _request;
        private readonly Fixture _fixture;

        public ElasticSearchWrapperTests()
        {
            _elasticClientMock = new Mock<IElasticClient>();

            _queryFactoryMock = new Mock<IQueryFactory>();
            _pagingHelperMock = new Mock<IPagingHelper>();
            _sortfactoryMock = new Mock<ISortFactory>();
            _loggerMock = new Mock<ILogger<ElasticSearchWrapper>>();
            _indexSelectorMock = new Mock<IIndexSelector>();
            _filterFactoryMock = new Mock<IFilterFactory>();

            _elasticSearchWrapper = new ElasticSearchWrapper(
                _elasticClientMock.Object,
                _queryFactoryMock.Object,
                _pagingHelperMock.Object,
                _sortfactoryMock.Object,
                _loggerMock.Object,
                _indexSelectorMock.Object,
                _filterFactoryMock.Object);

            _fixture = new Fixture();

            _request = _fixture.Create<GetAllTenureListRequest>();

            //add connection settings
            var nodes = _fixture.CreateMany<Node>().GetEnumerator();

            _elasticClientMock.Setup(x =>
                x.ConnectionSettings.ConnectionPool.Nodes.GetEnumerator()).Returns(nodes);

            //return default sort
            _sortfactoryMock.Setup(x =>
                x.Create<QueryableTenure, GetAllTenureListRequest>(_request)).Returns(new DefaultSort<QueryableTenure>());
        }

        [Fact]
        public async Task SearchTenuresSets_CallsESClientToGetNodeUrisFromConnectionSettings()
        {
            var result = await _elasticSearchWrapper
                .SearchTenuresSets(_request);

            _elasticClientMock.Verify(x =>
                x.ConnectionSettings.ConnectionPool.Nodes.GetEnumerator(), Times.Once());
        }

        [Fact]
        public async Task SearchTenuresSets_CallsLoggerToLogUsedESNodesAsDebugInfo()
        {
            var nodes = _fixture.CreateMany<Node>();

            var uris = string.Join(';', nodes.Select(x => x.Uri));

            var expectedLogMessage = $"ElasticSearch Search Tenures Sets begins {uris}";

            _elasticClientMock.Setup(x =>
                x.ConnectionSettings.ConnectionPool.Nodes.GetEnumerator()).Returns(nodes.GetEnumerator());

            var result = await _elasticSearchWrapper
                .SearchTenuresSets(_request);

            MockLoggerExtensions.VerifyExact(_loggerMock, LogLevel.Debug, expectedLogMessage, Times.Once());

        }

        [Fact]
        public async Task SearchTenuresSets_CallsCreateOnSortFactoryWithRequestToGetTheCorrectSortDescriptor()
        {
            var result = await _elasticSearchWrapper.SearchTenuresSets(_request);

            _sortfactoryMock.Verify(x => x.Create<QueryableTenure, GetAllTenureListRequest>(_request), Times.Once);
        }

        [Fact]
        public async Task SearchTenuresSets_ReturnsNoResultsWhenRequestIsNull()
        {
            GetAllTenureListRequest request = null;

            var result = await _elasticSearchWrapper
                .SearchTenuresSets(request);

            result.Documents.Count.Should().Be(0);
        }

        [Fact]
        public async Task SearchTenuresSets_CallsSearchAsyncOnESClient()
        {
            var mockResponse = new Mock<SearchResponse<QueryableTenure>>();

            _elasticClientMock.Setup(x =>
                x.SearchAsync(
                It.IsAny<Func<SearchDescriptor<QueryableTenure>, ISearchRequest>>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(mockResponse.Object);

            var result = await _elasticSearchWrapper.SearchTenuresSets(_request);

            _elasticClientMock.Verify(x =>
                x.SearchAsync(
                It.IsAny<Func<SearchDescriptor<QueryableTenure>, ISearchRequest>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchTenuresSets_ThrowsAnExceptionWhenESClientThrowsOne()
        {
            var ex = new Exception();

            _elasticClientMock.Setup(x =>
                x.SearchAsync(
                It.IsAny<Func<SearchDescriptor<QueryableTenure>, ISearchRequest>>(),
                It.IsAny<CancellationToken>())).Throws(ex);

            await _elasticSearchWrapper
                .Invoking(x => x.SearchTenuresSets(_request))
                .Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task SearchTenuresSets_LogsErrorWhenESClientThrows()
        {
            var ex = new Exception();
            var expectedLogMessage = "ElasticSearch Search Tenures Sets threw an exception";

            _elasticClientMock.Setup(x =>
                  x.SearchAsync(
                  It.IsAny<Func<SearchDescriptor<QueryableTenure>, ISearchRequest>>(),
                  It.IsAny<CancellationToken>())).Throws(ex);

            await _elasticSearchWrapper
                .Invoking(x => x.SearchTenuresSets(_request))
                .Should().ThrowAsync<Exception>();

            MockLoggerExtensions.VerifyExact(_loggerMock, LogLevel.Error, expectedLogMessage, Times.Once());
        }

        [Fact]
        public void GetLastSortedItem_ReturnsNullWhenLastHitIdAndLastHitTenureStartDateAreNotProvided()
        {
            var request = new GetAllTenureListRequest();

            var result = ElasticSearchWrapper.GetLastSortedItem(request);

            result.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetLastSortedItem_ReturnsNullWhenOnlyLastHitIdIsProvided(string lastHitTenureStartDate)
        {
            var request = new GetAllTenureListRequest()
            {
                LastHitId = Guid.NewGuid().ToString(),
                LastHitTenureStartDate = lastHitTenureStartDate
            };

            var result = ElasticSearchWrapper.GetLastSortedItem(request);

            result.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]

        public void GetLastSortedItem_ReturnsNullWhenOnlyLastHitTenureStartDateIsProvided(string lastHitId)
        {
            var request = new GetAllTenureListRequest()
            {
                LastHitTenureStartDate = "1717142746", //this must be in Unix epoch time
                LastHitId = lastHitId
            };

            var result = ElasticSearchWrapper.GetLastSortedItem(request);

            result.Should().BeNull();
        }

        [Fact]
        public void GetLastSortedItem_ReturnsCorrectArrayOfStringsWhenBothLastHitIdAndLastHitTenureStartDateAreProvided()
        {
            var request = new GetAllTenureListRequest()
            {
                LastHitTenureStartDate = "1717142746",
                LastHitId = Guid.NewGuid().ToString()
            };

            var result = ElasticSearchWrapper.GetLastSortedItem(request);

            result.Should().BeOfType(typeof(string[]));
            result.Length.Should().Be(2);
            result[0].Should().Be(request.LastHitTenureStartDate);
            result[1].Should().Be(request.LastHitId);
        }
    }
}
