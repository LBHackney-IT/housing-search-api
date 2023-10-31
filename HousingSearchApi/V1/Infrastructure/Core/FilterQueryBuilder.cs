using System;
using System.Collections.Generic;
using System.Linq;
using Hackney.Core.ElasticSearch.Interfaces;
using HousingSearchApi.V1.Interfaces;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Core
{
    public class FilterQueryBuilder<T> : IFilterQueryBuilder<T> where T : class
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;
        private Func<QueryContainerDescriptor<T>, QueryContainer> _wildstarQuery;
        private Func<QueryContainerDescriptor<T>, QueryContainer> _exactQuery;
        private List<List<Func<QueryContainerDescriptor<T>, QueryContainer>>> _filterQueries =
            new List<List<Func<QueryContainerDescriptor<T>, QueryContainer>>>();
        private List<Func<QueryContainerDescriptor<T>, QueryContainer>> _multipleFilterQueries =
            new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();


        public FilterQueryBuilder(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public IQueryBuilder<T> WithWildstarQuery(string searchText, List<string> fields, TextQueryType textQueryType = TextQueryType.MostFields)
        {
            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(searchText);
            var queryString = $"({string.Join(" AND ", listOfWildCardedWords)}) " +
                              string.Join(' ', listOfWildCardedWords);

            _wildstarQuery = CreateQuery(queryString, fields);

            return this;
        }

        public IQueryBuilder<T> WithFilterQuery(string commaSeparatedFilters, List<string> fields, TextQueryType textQueryType = TextQueryType.MostFields)
        {
            if (commaSeparatedFilters != null)
            {
                var filterQuery = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
                foreach (var filterWord in commaSeparatedFilters.Split(","))
                {
                    filterQuery.Add(CreateQuery(filterWord, fields));
                }
                _filterQueries.Add(filterQuery);
            }

            return this;
        }

        public IQueryBuilder<T> WithExactQuery(string searchText, List<string> fields,
            IExactSearchQuerystringProcessor processor = null, TextQueryType textQueryType = TextQueryType.MostFields)
        {
            if (processor != null)
                searchText = processor.Process(searchText);

            _exactQuery = CreateQuery(searchText, fields, 20);

            return this;
        }

        public IFilterQueryBuilder<T> WithMultipleFilterQuery(string commaSeparatedFilters, List<string> fields)
        {
            if (commaSeparatedFilters != null)
            {

                foreach (var filterWord in commaSeparatedFilters.Split(","))
                {
                    _multipleFilterQueries.Add(CreateQuery(filterWord, fields));
                }
            }

            return this;
        }

        private static Func<QueryContainerDescriptor<T>, QueryContainer> CreateQuery(string queryString,
            List<string> fields, double? boostValue = null)
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> query =
                (containerDescriptor) => containerDescriptor.QueryString(q =>
                {
                    var queryDescriptor = q.Query(queryString)
                        //.Type(TextQueryType.MostFields)
                        .Fields(f =>
                        {
                            foreach (var field in fields)
                            {
                                f = f.Field(field, boostValue);
                            }

                            return f;
                        });

                    return queryDescriptor;
                });

            return query;
        }

        public QueryContainer Build(QueryContainerDescriptor<T> containerDescriptor)
        {
            var queryContainer = containerDescriptor.Bool(x => x.Should(_wildstarQuery, _exactQuery));

            if (_multipleFilterQueries?.Any() == true)
            {
                var listOfMultiples = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
                /*
                 * We have to match ALL filter values, ie. a Must clause.
                 */
                queryContainer = containerDescriptor.Bool(x =>
                    x.Must(containerDescriptor.Bool(x => x.Must(listOfMultiples)),
                    queryContainer));
            }

            if (_filterQueries?.Any() == true)
            {
                /*
                 * Each field must be match, but it can match any one of
                 * of the filter values (a Should clause).
                 *
                 * In C# terms, the logic would look like:
                 * (field1 === filterValue1 || field1 === filterValue2)
                 *   && (field2 === filterValue3 || field2 === filterValue4) 
                 *   && ...
                 *
                 * This is suitable for fields like Asset Type or Tenure Type,
                 * where we want to return all matching assets from a set of types.
                 */
                foreach (var filterQuery in _filterQueries) {
                    var listOfFunctions = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
                    listOfFunctions.AddRange(filterQuery);
                    //Should with text search and multiple fields like AssetTypes
                    queryContainer = containerDescriptor.Bool(x =>
                        x.Must(containerDescriptor.Bool(x => x.Should(listOfFunctions)),
                        queryContainer));                   
                }
            }

            return queryContainer;
        }

        public QueryContainer BuildSimpleQuery(QueryContainerDescriptor<T> containerDescriptor, string searchTerm, List<string> fields)
        {
            return containerDescriptor.SimpleQueryString(q => q.Fields(f =>
            {
                foreach (var field in fields)
                {
                    f = f.Field(field);
                }
                return f;
            }
            ).Query(searchTerm));
        }
    }
}
