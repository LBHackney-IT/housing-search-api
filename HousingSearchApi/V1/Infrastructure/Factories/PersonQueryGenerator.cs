using System;
using HousingSearchApi.V1.Boundary.Requests;
using Nest;
using System.Collections.Generic;
using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using HousingSearchApi.V1.Infrastructure;
using HousingSearchApi.V1.Interfaces.Factories;

namespace HousingSearchApi.V1.Infrastructure.Factories
{
    public class PersonQueryGenerator : IQueryGenerator<QueryablePerson>
    {
        private readonly IQueryBuilder<QueryablePerson> _queryBuilder;

        public PersonQueryGenerator(IQueryBuilder<QueryablePerson> queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }



        public QueryContainer Create<TRequest>(TRequest request, QueryContainerDescriptor<QueryablePerson> q)
        {

            if (!(request is GetPersonListRequest personListRequest))
            {
                throw new ArgumentNullException($"{nameof(request).ToString()} shouldn't be null.");
            }

            _queryBuilder
                .WithWildstarQuery(personListRequest.SearchText,
                    new List<string> { "firstname", "surname" })
                .WithExactQuery(personListRequest.SearchText,
                    new List<string> { "firstname", "surname" }, new ExactSearchQuerystringProcessor());

            if (personListRequest.PersonType.HasValue)
            {
                _queryBuilder.WithFilterQuery(string.Join(",", personListRequest.PersonType.Value.GetPersonTypes()), new List<string> { "tenures.type" });
            }

            return _queryBuilder.Build(q);
        }
    }
}
