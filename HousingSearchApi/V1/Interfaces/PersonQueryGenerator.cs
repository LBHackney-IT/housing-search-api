using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Gateways.Models.Persons;
using HousingSearchApi.V1.Infrastructure;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousingSearchApi.V1.Interfaces
{
    public interface IQueryBuilder<T> where T : class
    {
        IQueryBuilder<T> WithQueryAndFields(string queryString, List<string> fields);

        QueryContainer FilterAndRespectSearchScore(QueryContainerDescriptor<T> descriptor);
    }

    public class QueryBuilder<T> : IQueryBuilder<T> where T : class
    {
        private readonly List<Func<QueryContainerDescriptor<T>, QueryContainer>> _queries;

        public QueryBuilder()
        {
            _queries = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
        }

        public IQueryBuilder<T> WithQueryAndFields(string queryString, List<string> fields)
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> query =
                (containerDescriptor) => containerDescriptor.QueryString(q =>
                {
                    var queryDescriptor = q.Query(queryString)
                        .Type(TextQueryType.MostFields)
                        .Fields(f =>
                        {
                            foreach (var field in fields)
                            {
                                f = f.Field(field);
                            }

                            return f;
                        });

                    return queryDescriptor;
                });

            _queries.Add(query);

            return this;
        }

        public QueryContainer FilterAndRespectSearchScore(QueryContainerDescriptor<T> containerDescriptor)
        {
            return containerDescriptor.Bool(builder => builder.Must(_queries));
        }
    }

    public class PersonQueryGenerator : IQueryGenerator<QueryablePerson>
    {
        private readonly IQueryBuilder<QueryablePerson> _queryBuilder;
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;

        public PersonQueryGenerator(IQueryBuilder<QueryablePerson> queryBuilder, IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _queryBuilder = queryBuilder;
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public QueryContainer Create(HousingSearchRequest request, QueryContainerDescriptor<QueryablePerson> containerDescriptor)
        {
            if (!(request is GetPersonListRequest personListRequest))
            {
                return null;
            }

            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(personListRequest.SearchText);
            var searchQuery = $"({string.Join(" AND ", listOfWildCardedWords)}) " +
                              string.Join(' ', listOfWildCardedWords);
            var searchFields = new List<string> { "firstname", "surname" };

            _queryBuilder
                .WithQueryAndFields(searchQuery, searchFields);

            if (personListRequest.PersonType.HasValue)
            {
                var filterQuery = string.Join(" ", personListRequest.PersonType.Value.GetPersonTypes());
                var filterFields = new List<string> {"tenures.type"};

                _queryBuilder
                    .WithQueryAndFields(filterQuery, filterFields);
            }

            return _queryBuilder
                .FilterAndRespectSearchScore(containerDescriptor);
        }
    }
}
