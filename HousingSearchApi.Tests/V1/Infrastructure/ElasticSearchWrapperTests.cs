using AutoFixture;
using Elasticsearch.Net;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Gateways.Models.Accounts;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Infrastructure;
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

namespace HousingSearchApi.Tests.V1.Infrastructure
{
    public class ElasticSearchWrapperTests
    {
        private readonly Mock<IElasticClient> _elasticClientMock;
        private readonly Mock<IQueryFactory> _queryFactoryMock;
        private readonly Mock<IPagingHelper> _pagingHelperMock;
        private readonly Mock<ISortFactory> _sorfactoryMock;
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
            _sorfactoryMock = new Mock<ISortFactory>();
            _loggerMock = new Mock<ILogger<ElasticSearchWrapper>>();
            _indexSelectorMock = new Mock<IIndexSelector>();
            _filterFactoryMock = new Mock<IFilterFactory>();

            _elasticSearchWrapper = new ElasticSearchWrapper(
                _elasticClientMock.Object,
                _queryFactoryMock.Object,
                _pagingHelperMock.Object,
                _sorfactoryMock.Object,
                _loggerMock.Object,
                _indexSelectorMock.Object,
                _filterFactoryMock.Object);

            _fixture = new Fixture();

            _request = _fixture.Create<GetAllTenureListRequest>();

            //add connection settings
            var nodes = _fixture.CreateMany<Node>().GetEnumerator();

            _elasticClientMock.Setup(x =>
                x.ConnectionSettings.ConnectionPool.Nodes.GetEnumerator()).Returns(nodes);
        }

        [Fact]
        public async Task SearchTenuresSets_ResultsTypeIsQueryableTenureAndQueryTypeIsGetAllTenureListRequest()
        {
            _ = await _elasticSearchWrapper
                .SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(_request);
        }

        [Fact]
        public async Task SearchTenuresSets_CallsESClientToGetNodeUrisFromConnectionSettings()
        {
            var result = await _elasticSearchWrapper
                .SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(_request);

            _elasticClientMock.Verify(x =>
                x.ConnectionSettings.ConnectionPool.Nodes.GetEnumerator(), Times.Once());
        }

        [Fact]
        public async Task SearchTenuresSets_CallsLoggerToLogESnodesUsedAsDebugInfo()
        {
            var nodes = _fixture.CreateMany<Node>();

            var uris = string.Join(';', nodes.Select(x => x.Uri));

            var expectedLogMessage = $"ElasticSearch Search Sets begins {uris}";

            _elasticClientMock.Setup(x =>
                x.ConnectionSettings.ConnectionPool.Nodes.GetEnumerator()).Returns(nodes.GetEnumerator());

            var result = await _elasticSearchWrapper
                .SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(_request);

            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Debug),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == expectedLogMessage && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }

        [Fact]
        public async Task SearchTenuresSets_ReturnsEmptyResultsWhenRequestIsNull()
        {
            var result = await _elasticSearchWrapper
                .SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(null);

            result.Documents.Count.Should().Be(0);
        }

        [Fact]
        public async Task SearchTenuresSets_CallsSearchAsyncOnESClient()
        {
            var mockResponse = new Mock<SearchResponse<QueryableTenure>>();

            _elasticClientMock.Setup(x =>
                x.SearchAsync<QueryableTenure>(
                It.IsAny<Func<SearchDescriptor<QueryableTenure>, ISearchRequest<QueryableTenure>>>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(mockResponse.Object);

            var result = await _elasticSearchWrapper
                .SearchTenuresSets<QueryableTenure, GetAllTenureListRequest>(_request);

            _elasticClientMock.Verify(x =>
                x.SearchAsync<QueryableTenure>(
                It.IsAny<Func<SearchDescriptor<QueryableTenure>, ISearchRequest<QueryableTenure>>>(),
                default(CancellationToken)), Times.Once);
        }
    }
}


