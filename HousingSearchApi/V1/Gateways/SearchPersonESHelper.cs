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
        private Indices.ManyIndices _indices;

        public SearchPersonESHelper(IElasticClient esClient)
        {
            _esClient = esClient;
            _indices = Indices.Index(new List<IndexName> { "persons" });
        }
        public async Task<ISearchResponse<QueryablePerson>> Search(GetPersonListRequest request)
        {
            try
            {
                LambdaLogger.Log(Environment.GetEnvironmentVariable("ELASTICSEARCH_DOMAIN_URL"));
                return await _esClient.SearchAsync<QueryablePerson>(x => x.Index(_indices)
                    .Query(q => BaseQuery(request, q))
                    .TrackTotalHits());
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                throw e;
            }
        }

        private QueryContainer BaseQuery(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            return SearchFirstNames(request, q)
                   || SearchLastNames(request, q);
        }

        private QueryContainer SearchPreferredFirstnames(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchPreferredFirstnames
                = q.Wildcard(m =>
                    m.Field(f => f.PreferredFirstname).Value($"*{searchText}*"));

            return searchPreferredFirstnames;
        }

        private QueryContainer SearchPreferredSurnames(GetPersonListRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchPreferredSurnames
                = q.Wildcard(m =>
                    m.Field(f => f.PreferredSurname).Value($"*{searchText}*"));

            return searchPreferredSurnames
                ;
        }

        private QueryContainer SearchFirstNames(GetPersonListRequest request,
            QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchFirstNames = q.Wildcard(m =>
                m.Field(f => f.Firstname).Value($"*{searchText}*"));

            return searchFirstNames;
        }

        private QueryContainer SearchLastNames(GetPersonListRequest request,
            QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchLastNames = q.Wildcard(m =>
                m.Field(f => f.Surname).Value($"*{searchText}*"));

            return searchLastNames;
        }

        private QueryContainer SearchMiddleNames(GetPersonListRequest request,
            QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchMiddleNames = q.Wildcard(m =>
                m.Field(f => f.MiddleName).Value($"*{searchText}*"));

            return searchMiddleNames
                ;
        }

        private QueryContainer SearchDateOfBirth(GetPersonListRequest request,
            QueryContainerDescriptor<QueryablePerson> q)
        {
            if (string.IsNullOrWhiteSpace(request.SearchText)) return null;
            var searchText = request.SearchText?.Replace(" ", "").ToLower();

            var searchDoB = q.Wildcard(m =>
                m.Field(f => f.DateOfBirth).Value($"*{searchText}*"));

            return searchDoB;
        }
    }
}
