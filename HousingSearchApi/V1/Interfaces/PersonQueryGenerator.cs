using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Persons;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;

namespace HousingSearchApi.V1.Interfaces
{
    public class PersonQueryGenerator : IQueryGenerator<QueryablePerson>
    {
        private readonly IQueryBuilder<QueryablePerson> _queryBuilder;

        public PersonQueryGenerator(IQueryBuilder<QueryablePerson> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {
            if (!(request is GetPersonListRequest personListRequest))
            {
                return null;
            }

            _queryBuilder.SpecifyFieldsToBeSearched(new List<string> { "firstname", "surname" })
                .CreateWildstarSearchQuery(personListRequest.SearchText);

            if (personListRequest.PersonType.HasValue)
            {
                _queryBuilder.SpecifyFieldsToBeFiltered(personListRequest.PersonType.Value.GetPersonTypes())
                    .CreateFilterQuery(personListRequest.SearchText);
            }

            return _queryBuilder.FilterAndRespectSearchScore(q);
        }
    }
}
