using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Gateways
{
    public class SearchPersonESHelper : ISearchPersonESHelper
    {
        private IElasticClient _esClient;
        private readonly ISearchPersonsQueryContainerOrchestrator _containerOrchestrator;
        private Indices.ManyIndices _indices;

        public SearchPersonESHelper(IElasticClient esClient, ISearchPersonsQueryContainerOrchestrator containerOrchestrator)
        {
            _esClient = esClient;
            _containerOrchestrator = containerOrchestrator;
            _indices = Indices.Index(new List<IndexName> { "persons" });
        }
        public async Task<ISearchResponse<QueryablePerson>> Search(GetPersonListRequest request)
        {
            try
            {
                LambdaLogger.Log("ES Search begins " + Environment.GetEnvironmentVariable("ELASTICSEARCH_DOMAIN_URL"));

                var result = await _esClient.SearchAsync<QueryablePerson>(x => x.Index(_indices)
                    .Query(q => BaseQuery(request, q))
                    .TrackTotalHits());

                LambdaLogger.Log("ES Search ended");

                return result;
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                throw e;
            }
        }

        private QueryContainer BaseQuery(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            return _containerOrchestrator.Create(request, q);
        }
    }
}
