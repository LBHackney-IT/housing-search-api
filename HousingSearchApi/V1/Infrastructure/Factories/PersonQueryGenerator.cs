using Hackney.Core.ElasticSearch.Interfaces;
using Hackney.Shared.HousingSearch.Domain;
using Hackney.Shared.HousingSearch.Gateways.Models.Persons;
using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Interfaces.Factories;
using Nest;
using System;
using System.Collections.Generic;

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
                    new List<string> { "firstname", "surname" }, new ExactSearchQuerystringProcessor())
                .WithFilterQuery($"NOT {PersonType.HousingOfficer}", new List<string> { "personTypes" });

            if (personListRequest.PersonType.HasValue)
            {
                _queryBuilder.WithFilterQuery(string.Join(",", personListRequest.PersonType.Value.GetPersonTypes()), new List<string> { "tenures.type" });
            }

            return _queryBuilder.Build(q);
        }
    }
}
