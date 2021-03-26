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

            return new GetPersonListResponse();
        }

        private QueryContainer BaseQuery(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            return SearchFirstNames(request, q)
                   || SearchLastNames(request, q)
                   || SearchMiddleNames(request, q)
                   || SearchDateOfBirth(request, q);
        }

        private QueryContainer SearchFirstNames(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchFirstNames = q.Wildcard(m =>
                m.Field(f => f.FirstName).Value($"*{searchText}*"));

            return searchFirstNames;
        }

        private QueryContainer SearchLastNames(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchLastNames = q.Wildcard(m =>
                m.Field(f => f.Surname).Value($"*{searchText}*"));

            return searchLastNames;
        }

        private QueryContainer SearchMiddleNames(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchMiddleNames = q.Wildcard(m =>
                m.Field(f => f.MiddleName).Value($"*{searchText}*"));

            return searchMiddleNames
;
        }

        private QueryContainer SearchDateOfBirth(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchDoB = q.Wildcard(m =>
                m.Field(f => f.DateOfBirth).Value($"*{searchText}*"));

            return searchDoB;
        }
    }
}
