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
        IQueryBuilder<T> CreateWildstarSearchQuery(string searchText);

        IQueryBuilder<T> CreateFilterQuery(string commaSeparatedFilters);

        IQueryBuilder<T> SpecifyFieldsToBeSearched(List<string> fields);

        IQueryBuilder<T> SpecifyFieldsToBeFiltered(List<string> fields);

        QueryContainer FilterAndRespectSearchScore(QueryContainerDescriptor<T> descriptor);

        QueryContainer Search(QueryContainerDescriptor<T> containerDescriptor);
    }

    public class QueryBuilder<T> : IQueryBuilder<T> where T : class
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;
        private readonly List<Func<QueryContainerDescriptor<T>, QueryContainer>> _queries;
        private string _searchQuery;
        private string _filterQuery;

        public QueryBuilder(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
            _queries = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
        }

        public IQueryBuilder<T> CreateWildstarSearchQuery(string searchText)
        {
            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(searchText);
            _searchQuery = $"({string.Join(" AND ", listOfWildCardedWords)}) " +
                              string.Join(' ', listOfWildCardedWords);

            return this;
        }

        public IQueryBuilder<T> CreateFilterQuery(string commaSeparatedFilters)
        {
            _filterQuery = string.Join(' ', commaSeparatedFilters.Split(","));

            return this;
        }

        public IQueryBuilder<T> SpecifyFieldsToBeSearched(List<string> fields)
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> query =
                (containerDescriptor) => containerDescriptor.QueryString(q =>
                {
                    var queryDescriptor = q.Query(_searchQuery)
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

        public IQueryBuilder<T> SpecifyFieldsToBeFiltered(List<string> fields)
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> query =
                (containerDescriptor) => containerDescriptor.QueryString(q =>
                {
                    var queryDescriptor = q.Query(_filterQuery)
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

        public QueryContainer Search(QueryContainerDescriptor<T> containerDescriptor)
        {
            return _queries.First().Invoke(containerDescriptor);
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
            var searchFields = new List<string> { "firstname", "surname" };

            _queryBuilder.CreateWildstarSearchQuery(request.SearchText)
                .SpecifyFieldsToBeSearched(searchFields);

            if (!string.IsNullOrWhiteSpace(request.AssetTypes))
            {
                var filterFields = new List<string> { "tenures.type" };

                _queryBuilder.CreateFilterQuery(request.AssetTypes)
                    .SpecifyFieldsToBeFiltered(filterFields);
            }

            return _queryBuilder.FilterAndRespectSearchScore(containerDescriptor);
        }
    }
}
