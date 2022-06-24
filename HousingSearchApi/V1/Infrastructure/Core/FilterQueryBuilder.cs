using System;
using System.Collections.Generic;
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
        private List<Func<QueryContainerDescriptor<T>, QueryContainer>> _filterQueries;
        private List<Func<QueryContainerDescriptor<T>, QueryContainer>> _multipleFilterQueries =
            new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();

        
        public FilterQueryBuilder(IWildCardAppenderAndPrepender wildCardAppenderAndPrepender)
        {
            _wildCardAppenderAndPrepender = wildCardAppenderAndPrepender;
        }

        public IQueryBuilder<T> WithWildstarQuery(string searchText, List<string> fields)
        {
            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(searchText);
            var queryString = $"({string.Join(" AND ", listOfWildCardedWords)}) " +
                              string.Join(' ', listOfWildCardedWords);

            _wildstarQuery = CreateQuery(queryString, fields);

            return this;
        }

        public IQueryBuilder<T> WithFilterQuery(string commaSeparatedFilters, List<string> fields)
        {
            if (commaSeparatedFilters != null)
            {
                _filterQueries = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
                foreach (var filterWord in commaSeparatedFilters.Split(","))
                {
                    _filterQueries.Add(CreateQuery(filterWord, fields));
                }
            }

            return this;
        }

        public IQueryBuilder<T> WithExactQuery(string searchText, List<string> fields,
            IExactSearchQuerystringProcessor processor = null)
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
            var queryContainer = containerDescriptor.Bool(x => x.Must(_wildstarQuery, _exactQuery));

            if (_multipleFilterQueries != null)
            {
                var listOfMultiples = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
                listOfMultiples.AddRange(_multipleFilterQueries);
                //Must with multiple filter queries
                queryContainer = containerDescriptor.Bool(x =>
                    x.Must(containerDescriptor.Bool(x => x.Must(listOfMultiples)),
                    queryContainer));
            }

            if (_filterQueries != null)
            {
                var listOfFunctions = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
                listOfFunctions.AddRange(_filterQueries);
                //Should with text search and multiple fields like AssetTypes
                queryContainer = containerDescriptor.Bool(x =>
                    x.Must(containerDescriptor.Bool(x => x.Should(listOfFunctions)),
                    queryContainer));
            }

            return queryContainer;
        }
    }
}
