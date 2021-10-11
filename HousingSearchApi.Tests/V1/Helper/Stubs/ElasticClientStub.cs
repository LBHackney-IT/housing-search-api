using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using Nest.Specification.AsyncSearchApi;
using Nest.Specification.CatApi;
using Nest.Specification.ClusterApi;
using Nest.Specification.CrossClusterReplicationApi;
using Nest.Specification.DanglingIndicesApi;
using Nest.Specification.EnrichApi;
using Nest.Specification.EqlApi;
using Nest.Specification.GraphApi;
using Nest.Specification.IndexLifecycleManagementApi;
using Nest.Specification.IndicesApi;
using Nest.Specification.IngestApi;
using Nest.Specification.LicenseApi;
using Nest.Specification.MachineLearningApi;
using Nest.Specification.MigrationApi;
using Nest.Specification.NodesApi;
using Nest.Specification.RollupApi;
using Nest.Specification.SecurityApi;
using Nest.Specification.SnapshotApi;
using Nest.Specification.SnapshotLifecycleManagementApi;
using Nest.Specification.SqlApi;
using Nest.Specification.TasksApi;
using Nest.Specification.TransformApi;
using Nest.Specification.WatcherApi;
using Nest.Specification.XPackApi;

namespace HousingSearchApi.Tests.V1.Helper.Stubs
{
    public class ElasticClientStub : IElasticClient
    {
        public BulkAllObservable<T> BulkAll<T>(IEnumerable<T> documents, Func<BulkAllDescriptor<T>, IBulkAllRequest<T>> selector,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            throw new NotImplementedException();
        }

        public BulkAllObservable<T> BulkAll<T>(IBulkAllRequest<T> request, CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            throw new NotImplementedException();
        }

        public IObservable<BulkAllResponse> Reindex<TSource, TTarget>(Func<TSource, TTarget> mapper, Func<ReindexDescriptor<TSource, TTarget>, IReindexRequest<TSource, TTarget>> selector,
            CancellationToken cancellationToken = new CancellationToken()) where TSource : class where TTarget : class
        {
            throw new NotImplementedException();
        }

        public IObservable<BulkAllResponse> Reindex<TSource>(Func<ReindexDescriptor<TSource, TSource>, IReindexRequest<TSource, TSource>> selector, CancellationToken cancellationToken = new CancellationToken()) where TSource : class
        {
            throw new NotImplementedException();
        }

        public IObservable<BulkAllResponse> Reindex<TSource, TTarget>(IReindexRequest<TSource, TTarget> request,
            CancellationToken cancellationToken = new CancellationToken()) where TSource : class where TTarget : class
        {
            throw new NotImplementedException();
        }

        public IObservable<BulkAllResponse> Reindex<TSource>(IReindexRequest<TSource> request, CancellationToken cancellationToken = new CancellationToken()) where TSource : class
        {
            throw new NotImplementedException();
        }

        public IObservable<BulkAllResponse> Reindex<TSource, TTarget>(IndexName fromIndex, IndexName toIndex, Func<TSource, TTarget> mapper, Func<QueryContainerDescriptor<TSource>, QueryContainer> selector = null,
            CancellationToken cancellationToken = new CancellationToken()) where TSource : class where TTarget : class
        {
            throw new NotImplementedException();
        }

        public IObservable<BulkAllResponse> Reindex<TSource>(IndexName fromIndex, IndexName toIndex, Func<QueryContainerDescriptor<TSource>, QueryContainer> selector = null,
            CancellationToken cancellationToken = new CancellationToken()) where TSource : class
        {
            throw new NotImplementedException();
        }

        public IObservable<ScrollAllResponse<T>> ScrollAll<T>(Time scrollTime, int numberOfSlices, Func<ScrollAllDescriptor<T>, IScrollAllRequest> selector = null,
            CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            throw new NotImplementedException();
        }

        public IObservable<ScrollAllResponse<T>> ScrollAll<T>(IScrollAllRequest request, CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            throw new NotImplementedException();
        }

        public CreateResponse CreateDocument<TDocument>(TDocument document) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<CreateResponse> CreateDocumentAsync<TDocument>(TDocument document, CancellationToken cancellationToken = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public IndexResponse IndexDocument<TDocument>(TDocument document) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<IndexResponse> IndexDocumentAsync<T>(T document, CancellationToken ct = new CancellationToken()) where T : class
        {
            throw new NotImplementedException();
        }

        public BulkResponse Bulk(Func<BulkDescriptor, IBulkRequest> selector)
        {
            throw new NotImplementedException();
        }

        public Task<BulkResponse> BulkAsync(Func<BulkDescriptor, IBulkRequest> selector, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public BulkResponse Bulk(IBulkRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BulkResponse> BulkAsync(IBulkRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ClearScrollResponse ClearScroll(Func<ClearScrollDescriptor, IClearScrollRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<ClearScrollResponse> ClearScrollAsync(Func<ClearScrollDescriptor, IClearScrollRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ClearScrollResponse ClearScroll(IClearScrollRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ClearScrollResponse> ClearScrollAsync(IClearScrollRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ClosePointInTimeResponse ClosePointInTime(Func<ClosePointInTimeDescriptor, IClosePointInTimeRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<ClosePointInTimeResponse> ClosePointInTimeAsync(Func<ClosePointInTimeDescriptor, IClosePointInTimeRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ClosePointInTimeResponse ClosePointInTime(IClosePointInTimeRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ClosePointInTimeResponse> ClosePointInTimeAsync(IClosePointInTimeRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public CountResponse Count<TDocument>(Func<CountDescriptor<TDocument>, ICountRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<CountResponse> CountAsync<TDocument>(Func<CountDescriptor<TDocument>, ICountRequest> selector = null, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public CountResponse Count(ICountRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<CountResponse> CountAsync(ICountRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public CreateResponse Create<TDocument>(TDocument document, Func<CreateDescriptor<TDocument>, ICreateRequest<TDocument>> selector) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<CreateResponse> CreateAsync<TDocument>(TDocument document, Func<CreateDescriptor<TDocument>, ICreateRequest<TDocument>> selector, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public CreateResponse Create<TDocument>(ICreateRequest<TDocument> request) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<CreateResponse> CreateAsync<TDocument>(ICreateRequest<TDocument> request, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public DeleteResponse Delete<TDocument>(DocumentPath<TDocument> id, Func<DeleteDescriptor<TDocument>, IDeleteRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<DeleteResponse> DeleteAsync<TDocument>(DocumentPath<TDocument> id, Func<DeleteDescriptor<TDocument>, IDeleteRequest> selector = null, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public DeleteResponse Delete(IDeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteResponse> DeleteAsync(IDeleteRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public DeleteByQueryResponse DeleteByQuery<TDocument>(Func<DeleteByQueryDescriptor<TDocument>, IDeleteByQueryRequest> selector) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<DeleteByQueryResponse> DeleteByQueryAsync<TDocument>(Func<DeleteByQueryDescriptor<TDocument>, IDeleteByQueryRequest> selector, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public DeleteByQueryResponse DeleteByQuery(IDeleteByQueryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteByQueryResponse> DeleteByQueryAsync(IDeleteByQueryRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ListTasksResponse DeleteByQueryRethrottle(TaskId taskId, Func<DeleteByQueryRethrottleDescriptor, IDeleteByQueryRethrottleRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<ListTasksResponse> DeleteByQueryRethrottleAsync(TaskId taskId, Func<DeleteByQueryRethrottleDescriptor, IDeleteByQueryRethrottleRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ListTasksResponse DeleteByQueryRethrottle(IDeleteByQueryRethrottleRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ListTasksResponse> DeleteByQueryRethrottleAsync(IDeleteByQueryRethrottleRequest request,
            CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public DeleteScriptResponse DeleteScript(Id id, Func<DeleteScriptDescriptor, IDeleteScriptRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteScriptResponse> DeleteScriptAsync(Id id, Func<DeleteScriptDescriptor, IDeleteScriptRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public DeleteScriptResponse DeleteScript(IDeleteScriptRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteScriptResponse> DeleteScriptAsync(IDeleteScriptRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ExistsResponse DocumentExists<TDocument>(DocumentPath<TDocument> id, Func<DocumentExistsDescriptor<TDocument>, IDocumentExistsRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ExistsResponse> DocumentExistsAsync<TDocument>(DocumentPath<TDocument> id, Func<DocumentExistsDescriptor<TDocument>, IDocumentExistsRequest> selector = null,
            CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public ExistsResponse DocumentExists(IDocumentExistsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ExistsResponse> DocumentExistsAsync(IDocumentExistsRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ExistsResponse SourceExists<TDocument>(DocumentPath<TDocument> id, Func<SourceExistsDescriptor<TDocument>, ISourceExistsRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ExistsResponse> SourceExistsAsync<TDocument>(DocumentPath<TDocument> id, Func<SourceExistsDescriptor<TDocument>, ISourceExistsRequest> selector = null,
            CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public ExistsResponse SourceExists(ISourceExistsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ExistsResponse> SourceExistsAsync(ISourceExistsRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ExplainResponse<TDocument> Explain<TDocument>(DocumentPath<TDocument> id, Func<ExplainDescriptor<TDocument>, IExplainRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ExplainResponse<TDocument>> ExplainAsync<TDocument>(DocumentPath<TDocument> id, Func<ExplainDescriptor<TDocument>, IExplainRequest> selector = null, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public ExplainResponse<TDocument> Explain<TDocument>(IExplainRequest request) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ExplainResponse<TDocument>> ExplainAsync<TDocument>(IExplainRequest request, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public FieldCapabilitiesResponse FieldCapabilities(Indices index = null, Func<FieldCapabilitiesDescriptor, IFieldCapabilitiesRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<FieldCapabilitiesResponse> FieldCapabilitiesAsync(Indices index = null, Func<FieldCapabilitiesDescriptor, IFieldCapabilitiesRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public FieldCapabilitiesResponse FieldCapabilities(IFieldCapabilitiesRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<FieldCapabilitiesResponse> FieldCapabilitiesAsync(IFieldCapabilitiesRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public GetResponse<TDocument> Get<TDocument>(DocumentPath<TDocument> id, Func<GetDescriptor<TDocument>, IGetRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<GetResponse<TDocument>> GetAsync<TDocument>(DocumentPath<TDocument> id, Func<GetDescriptor<TDocument>, IGetRequest> selector = null, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public GetResponse<TDocument> Get<TDocument>(IGetRequest request) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<GetResponse<TDocument>> GetAsync<TDocument>(IGetRequest request, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public GetScriptResponse GetScript(Id id, Func<GetScriptDescriptor, IGetScriptRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<GetScriptResponse> GetScriptAsync(Id id, Func<GetScriptDescriptor, IGetScriptRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public GetScriptResponse GetScript(IGetScriptRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<GetScriptResponse> GetScriptAsync(IGetScriptRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public SourceResponse<TDocument> Source<TDocument>(DocumentPath<TDocument> id, Func<SourceDescriptor<TDocument>, ISourceRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<SourceResponse<TDocument>> SourceAsync<TDocument>(DocumentPath<TDocument> id, Func<SourceDescriptor<TDocument>, ISourceRequest> selector = null, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public SourceResponse<TDocument> Source<TDocument>(ISourceRequest request) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<SourceResponse<TDocument>> SourceAsync<TDocument>(ISourceRequest request, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public IndexResponse Index<TDocument>(TDocument document, Func<IndexDescriptor<TDocument>, IIndexRequest<TDocument>> selector) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<IndexResponse> IndexAsync<TDocument>(TDocument document, Func<IndexDescriptor<TDocument>, IIndexRequest<TDocument>> selector, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public IndexResponse Index<TDocument>(IIndexRequest<TDocument> request) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<IndexResponse> IndexAsync<TDocument>(IIndexRequest<TDocument> request, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public RootNodeInfoResponse RootNodeInfo(Func<RootNodeInfoDescriptor, IRootNodeInfoRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<RootNodeInfoResponse> RootNodeInfoAsync(Func<RootNodeInfoDescriptor, IRootNodeInfoRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public RootNodeInfoResponse RootNodeInfo(IRootNodeInfoRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<RootNodeInfoResponse> RootNodeInfoAsync(IRootNodeInfoRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public MultiGetResponse MultiGet(Func<MultiGetDescriptor, IMultiGetRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<MultiGetResponse> MultiGetAsync(Func<MultiGetDescriptor, IMultiGetRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public MultiGetResponse MultiGet(IMultiGetRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<MultiGetResponse> MultiGetAsync(IMultiGetRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public MultiSearchResponse MultiSearch(Indices index = null, Func<MultiSearchDescriptor, IMultiSearchRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<MultiSearchResponse> MultiSearchAsync(Indices index = null, Func<MultiSearchDescriptor, IMultiSearchRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public MultiSearchResponse MultiSearch(IMultiSearchRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<MultiSearchResponse> MultiSearchAsync(IMultiSearchRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public MultiSearchResponse MultiSearchTemplate(Indices index = null, Func<MultiSearchTemplateDescriptor, IMultiSearchTemplateRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<MultiSearchResponse> MultiSearchTemplateAsync(Indices index = null, Func<MultiSearchTemplateDescriptor, IMultiSearchTemplateRequest> selector = null,
            CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public MultiSearchResponse MultiSearchTemplate(IMultiSearchTemplateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<MultiSearchResponse> MultiSearchTemplateAsync(IMultiSearchTemplateRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public MultiTermVectorsResponse MultiTermVectors(Func<MultiTermVectorsDescriptor, IMultiTermVectorsRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<MultiTermVectorsResponse> MultiTermVectorsAsync(Func<MultiTermVectorsDescriptor, IMultiTermVectorsRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public MultiTermVectorsResponse MultiTermVectors(IMultiTermVectorsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<MultiTermVectorsResponse> MultiTermVectorsAsync(IMultiTermVectorsRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public OpenPointInTimeResponse OpenPointInTime(Indices index = null, Func<OpenPointInTimeDescriptor, IOpenPointInTimeRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<OpenPointInTimeResponse> OpenPointInTimeAsync(Indices index = null, Func<OpenPointInTimeDescriptor, IOpenPointInTimeRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public OpenPointInTimeResponse OpenPointInTime(IOpenPointInTimeRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<OpenPointInTimeResponse> OpenPointInTimeAsync(IOpenPointInTimeRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public PingResponse Ping(Func<PingDescriptor, IPingRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<PingResponse> PingAsync(Func<PingDescriptor, IPingRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public PingResponse Ping(IPingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PingResponse> PingAsync(IPingRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public PutScriptResponse PutScript(Id id, Func<PutScriptDescriptor, IPutScriptRequest> selector)
        {
            throw new NotImplementedException();
        }

        public Task<PutScriptResponse> PutScriptAsync(Id id, Func<PutScriptDescriptor, IPutScriptRequest> selector, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public PutScriptResponse PutScript(IPutScriptRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PutScriptResponse> PutScriptAsync(IPutScriptRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ReindexOnServerResponse ReindexOnServer(Func<ReindexOnServerDescriptor, IReindexOnServerRequest> selector)
        {
            throw new NotImplementedException();
        }

        public Task<ReindexOnServerResponse> ReindexOnServerAsync(Func<ReindexOnServerDescriptor, IReindexOnServerRequest> selector, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ReindexOnServerResponse ReindexOnServer(IReindexOnServerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ReindexOnServerResponse> ReindexOnServerAsync(IReindexOnServerRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ReindexRethrottleResponse ReindexRethrottle(TaskId taskId, Func<ReindexRethrottleDescriptor, IReindexRethrottleRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<ReindexRethrottleResponse> ReindexRethrottleAsync(TaskId taskId, Func<ReindexRethrottleDescriptor, IReindexRethrottleRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ReindexRethrottleResponse ReindexRethrottle(IReindexRethrottleRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ReindexRethrottleResponse> ReindexRethrottleAsync(IReindexRethrottleRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public RenderSearchTemplateResponse RenderSearchTemplate(Func<RenderSearchTemplateDescriptor, IRenderSearchTemplateRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<RenderSearchTemplateResponse> RenderSearchTemplateAsync(Func<RenderSearchTemplateDescriptor, IRenderSearchTemplateRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public RenderSearchTemplateResponse RenderSearchTemplate(IRenderSearchTemplateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<RenderSearchTemplateResponse> RenderSearchTemplateAsync(IRenderSearchTemplateRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ExecutePainlessScriptResponse<TResult> ExecutePainlessScript<TResult>(Func<ExecutePainlessScriptDescriptor, IExecutePainlessScriptRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutePainlessScriptResponse<TResult>> ExecutePainlessScriptAsync<TResult>(Func<ExecutePainlessScriptDescriptor, IExecutePainlessScriptRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ExecutePainlessScriptResponse<TResult> ExecutePainlessScript<TResult>(IExecutePainlessScriptRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutePainlessScriptResponse<TResult>> ExecutePainlessScriptAsync<TResult>(IExecutePainlessScriptRequest request,
            CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ISearchResponse<TDocument> Scroll<TInferDocument, TDocument>(Time scroll, string scrollId, Func<ScrollDescriptor<TInferDocument>, IScrollRequest> selector = null) where TInferDocument : class where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ISearchResponse<TDocument>> ScrollAsync<TInferDocument, TDocument>(Time scroll, string scrollId, Func<ScrollDescriptor<TInferDocument>, IScrollRequest> selector = null,
            CancellationToken ct = new CancellationToken()) where TInferDocument : class where TDocument : class
        {
            throw new NotImplementedException();
        }

        public ISearchResponse<TDocument> Scroll<TDocument>(Time scroll, string scrollId, Func<ScrollDescriptor<TDocument>, IScrollRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ISearchResponse<TDocument>> ScrollAsync<TDocument>(Time scroll, string scrollId, Func<ScrollDescriptor<TDocument>, IScrollRequest> selector = null,
            CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public ISearchResponse<TDocument> Scroll<TDocument>(IScrollRequest request) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ISearchResponse<TDocument>> ScrollAsync<TDocument>(IScrollRequest request, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public ISearchResponse<TDocument> Search<TInferDocument, TDocument>(Func<SearchDescriptor<TInferDocument>, ISearchRequest> selector = null) where TInferDocument : class where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ISearchResponse<TDocument>> SearchAsync<TInferDocument, TDocument>(Func<SearchDescriptor<TInferDocument>, ISearchRequest> selector = null, CancellationToken ct = new CancellationToken()) where TInferDocument : class where TDocument : class
        {
            return Task.FromResult((ISearchResponse<TDocument>) new SearchResponse<TDocument>());
        }

        public ISearchResponse<TDocument> Search<TDocument>(Func<SearchDescriptor<TDocument>, ISearchRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ISearchResponse<TDocument>> SearchAsync<TDocument>(Func<SearchDescriptor<TDocument>, ISearchRequest> selector = null, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            return Task.FromResult((ISearchResponse<TDocument>) new SearchResponse<TDocument>());
        }

        public ISearchResponse<TDocument> Search<TDocument>(ISearchRequest request) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ISearchResponse<TDocument>> SearchAsync<TDocument>(ISearchRequest request, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            return Task.FromResult((ISearchResponse<TDocument>) new SearchResponse<TDocument>());
        }

        public SearchShardsResponse SearchShards<TDocument>(Func<SearchShardsDescriptor<TDocument>, ISearchShardsRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<SearchShardsResponse> SearchShardsAsync<TDocument>(Func<SearchShardsDescriptor<TDocument>, ISearchShardsRequest> selector = null, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public SearchShardsResponse SearchShards(ISearchShardsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SearchShardsResponse> SearchShardsAsync(ISearchShardsRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ISearchResponse<TDocument> SearchTemplate<TDocument>(Func<SearchTemplateDescriptor<TDocument>, ISearchTemplateRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ISearchResponse<TDocument>> SearchTemplateAsync<TDocument>(Func<SearchTemplateDescriptor<TDocument>, ISearchTemplateRequest> selector = null, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public ISearchResponse<TDocument> SearchTemplate<TDocument>(ISearchTemplateRequest request) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<ISearchResponse<TDocument>> SearchTemplateAsync<TDocument>(ISearchTemplateRequest request, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public TermVectorsResponse TermVectors<TDocument>(Func<TermVectorsDescriptor<TDocument>, ITermVectorsRequest<TDocument>> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<TermVectorsResponse> TermVectorsAsync<TDocument>(Func<TermVectorsDescriptor<TDocument>, ITermVectorsRequest<TDocument>> selector = null, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public TermVectorsResponse TermVectors<TDocument>(ITermVectorsRequest<TDocument> request) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<TermVectorsResponse> TermVectorsAsync<TDocument>(ITermVectorsRequest<TDocument> request, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public UpdateResponse<TDocument> Update<TDocument, TPartialDocument>(DocumentPath<TDocument> id, Func<UpdateDescriptor<TDocument, TPartialDocument>, IUpdateRequest<TDocument, TPartialDocument>> selector) where TDocument : class where TPartialDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResponse<TDocument>> UpdateAsync<TDocument, TPartialDocument>(DocumentPath<TDocument> id, Func<UpdateDescriptor<TDocument, TPartialDocument>, IUpdateRequest<TDocument, TPartialDocument>> selector,
            CancellationToken ct = new CancellationToken()) where TDocument : class where TPartialDocument : class
        {
            throw new NotImplementedException();
        }

        public UpdateResponse<TDocument> Update<TDocument>(DocumentPath<TDocument> id, Func<UpdateDescriptor<TDocument, TDocument>, IUpdateRequest<TDocument, TDocument>> selector) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResponse<TDocument>> UpdateAsync<TDocument>(DocumentPath<TDocument> id, Func<UpdateDescriptor<TDocument, TDocument>, IUpdateRequest<TDocument, TDocument>> selector, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public UpdateResponse<TDocument> Update<TDocument, TPartialDocument>(IUpdateRequest<TDocument, TPartialDocument> request) where TDocument : class where TPartialDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResponse<TDocument>> UpdateAsync<TDocument, TPartialDocument>(IUpdateRequest<TDocument, TPartialDocument> request, CancellationToken ct = new CancellationToken()) where TDocument : class where TPartialDocument : class
        {
            throw new NotImplementedException();
        }

        public UpdateByQueryResponse UpdateByQuery<TDocument>(Func<UpdateByQueryDescriptor<TDocument>, IUpdateByQueryRequest> selector = null) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public Task<UpdateByQueryResponse> UpdateByQueryAsync<TDocument>(Func<UpdateByQueryDescriptor<TDocument>, IUpdateByQueryRequest> selector = null, CancellationToken ct = new CancellationToken()) where TDocument : class
        {
            throw new NotImplementedException();
        }

        public UpdateByQueryResponse UpdateByQuery(IUpdateByQueryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateByQueryResponse> UpdateByQueryAsync(IUpdateByQueryRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ListTasksResponse UpdateByQueryRethrottle(TaskId taskId, Func<UpdateByQueryRethrottleDescriptor, IUpdateByQueryRethrottleRequest> selector = null)
        {
            throw new NotImplementedException();
        }

        public Task<ListTasksResponse> UpdateByQueryRethrottleAsync(TaskId taskId, Func<UpdateByQueryRethrottleDescriptor, IUpdateByQueryRethrottleRequest> selector = null, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ListTasksResponse UpdateByQueryRethrottle(IUpdateByQueryRethrottleRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ListTasksResponse> UpdateByQueryRethrottleAsync(IUpdateByQueryRethrottleRequest request,
            CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public PutMappingResponse Map<T>(Func<PutMappingDescriptor<T>, IPutMappingRequest> selector) where T : class
        {
            throw new NotImplementedException();
        }

        public PutMappingResponse Map(IPutMappingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PutMappingResponse> MapAsync<T>(Func<PutMappingDescriptor<T>, IPutMappingRequest> selector, CancellationToken ct = new CancellationToken()) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<PutMappingResponse> MapAsync(IPutMappingRequest request, CancellationToken ct = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public IConnectionSettingsValues ConnectionSettings { get; }
        public Inferrer Infer { get; }
        public IElasticLowLevelClient LowLevel { get; }
        public IElasticsearchSerializer RequestResponseSerializer { get; }
        public IElasticsearchSerializer SourceSerializer { get; }
        public AsyncSearchNamespace AsyncSearch { get; }
        public CatNamespace Cat { get; }
        public ClusterNamespace Cluster { get; }
        public CrossClusterReplicationNamespace CrossClusterReplication { get; }
        public DanglingIndicesNamespace DanglingIndices { get; }
        public EnrichNamespace Enrich { get; }
        public EqlNamespace Eql { get; }
        public GraphNamespace Graph { get; }
        public IndexLifecycleManagementNamespace IndexLifecycleManagement { get; }
        public IndicesNamespace Indices { get; }
        public IngestNamespace Ingest { get; }
        public LicenseNamespace License { get; }
        public MachineLearningNamespace MachineLearning { get; }
        public MigrationNamespace Migration { get; }
        public NodesNamespace Nodes { get; }
        public RollupNamespace Rollup { get; }
        public SecurityNamespace Security { get; }
        public SnapshotNamespace Snapshot { get; }
        public SnapshotLifecycleManagementNamespace SnapshotLifecycleManagement { get; }
        public SqlNamespace Sql { get; }
        public TasksNamespace Tasks { get; }
        public TransformNamespace Transform { get; }
        public WatcherNamespace Watcher { get; }
        public XPackNamespace XPack { get; }
    }
}
