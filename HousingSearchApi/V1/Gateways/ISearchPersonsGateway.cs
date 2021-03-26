using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Infrastructure;
using Nest;

namespace HousingSearchApi.V1.Gateways
{
    public interface ISearchPersonsGateway
    {
        Task<GetPersonListResponse> GetListOfPersons(GetPersonListRequest query);
    }

    public class SearchPersonsGateway : ISearchPersonsGateway
    {
        private readonly IElasticClient _esClient;
        private Indices.ManyIndices _indices;

        public SearchPersonsGateway(IElasticClient esClient)
        {
            _esClient = esClient;
            _indices = Indices.Index(new List<IndexName> {"persons"});
        }

        public async Task<GetPersonListResponse> GetListOfPersons(GetPersonListRequest request)
        {
            var searchResponse = await _esClient.SearchAsync<QueryablePerson>(x => x.Index(_indices)
                .Query(q => BaseQuery(request, q))
                .TrackTotalHits());

            throw new NotImplementedException();
        }

        private QueryContainer BaseQuery(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            return SearchPostcodes(request, q)
                   && SearchBuildingNumbers(request, q)
                   && SearchAddressStatuses(request, q)
                   && SearchUsageCodes(request, q);
        }

        private QueryContainer SearchPostcodes(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            throw new System.NotImplementedException();
        }

        private QueryContainer SearchBuildingNumbers(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            throw new System.NotImplementedException();
        }

        private QueryContainer SearchAddressStatuses(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            throw new System.NotImplementedException();
        }

        private QueryContainer SearchUsageCodes(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            throw new System.NotImplementedException();
        }
    }
}
