using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime.Internal;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;

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

            _queryBuilder
                .WithWildstarQuery(request.SearchText,
                    new List<string> { "firstname", "surname" })
                .WithExactQuery(request.SearchText,
                    new List<string> { "firstname", "surname" }, new ExactSearchQuerystringProcessor());

            if (personListRequest.PersonType.HasValue)
            {
                _queryBuilder.WithFilterQuery(string.Join(",", personListRequest.PersonType.Value.GetPersonTypes()), new List<string> { "tenures.type" });
            }

            return _queryBuilder.Build(q);
        }
    }
}
