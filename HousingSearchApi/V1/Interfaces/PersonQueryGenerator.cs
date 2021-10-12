using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Persons;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using System.Collections.Generic;
using System.Linq;
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

            _queryBuilder
                .WithWildstarQuery(request.SearchText,
                    new List<string> { "firstname", "surname" })
                .WithExactQuery(request.SearchText,
                    new List<string> { "firstname", "surname" }, new ExactSearchQuerystringProcessor());

            if (personListRequest.PersonType.HasValue)
            {
                _queryBuilder.WithFilterQuery(request.AssetTypes, personListRequest.PersonType.Value.GetPersonTypes());
            }

            return _queryBuilder.Build(q);
        }
    }

    public class ExactSearchQuerystringProcessor : IExactSearchQuerystringProcessor
    {
        public string Process(string searchText)
        {
            if (searchText.Split(" ").Length > 0)
                return searchText.Replace(" ", " AND ");

            return searchText;
        }
    }

}
