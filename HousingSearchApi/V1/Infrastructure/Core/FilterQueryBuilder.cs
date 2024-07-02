using System;
using System.Collections.Generic;
using System.Linq;
using Hackney.Core.ElasticSearch.Interfaces;
using HousingSearchApi.V1.Interfaces;
using Nest;

namespace HousingSearchApi.V1.Infrastructure.Core
{
    /// <remarks>
    /// Note: this class was copied from <see cref="Hackney.Core.ElasticSearch.QueryBuilder{T}"/> 
    /// in <see href="https://github.com/LBHackney-IT/housing-search-api/pull/126">PR 126</see>
    /// and a new interface <see cref="IFilterQueryBuilder{T}"/>
    /// defined to allow successive <c>Must</c> clauses.
    /// But successive <c>Must</c> clauses are already possible by repeated calls to
    /// <see cref="IQueryBuilder{T}.WithFilterQuery"/>.
    /// A possible use case could be applying <c>Must</c> clauses to fields
    /// that repeat in the document - but it is unclear whether
    /// this is a genuine use case for the organisation.
    /// A future analysis piece should be to determine the need for this
    /// class, and revert to using <see cref="Hackney.Core.ElasticSearch.QueryBuilder{T}"/>
    /// as appropriate.
    /// </remarks>
    public class FilterQueryBuilder<T> : IFilterQueryBuilder<T> where T : class
    {
        private readonly IWildCardAppenderAndPrepender _wildCardAppenderAndPrepender;
        private Func<QueryContainerDescriptor<T>, QueryContainer> _wildstarQuery;
        private Func<QueryContainerDescriptor<T>, QueryContainer> _exactQuery;
        private readonly List<List<Func<QueryContainerDescriptor<T>, QueryContainer>>> _filterQueries =
            new List<List<Func<QueryContainerDescriptor<T>, QueryContainer>>>();
        private readonly List<Func<QueryContainerDescriptor<T>, QueryContainer>> _multipleFilterQueries =
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
        public IFilterQueryBuilder<T> WithWildstarBoolQuery(string searchText, List<string> fields, int? minimumShouldMatch = 1, TextQueryType textQueryType = TextQueryType.MostFields)
        {
            var listOfWildCardedWords = _wildCardAppenderAndPrepender.Process(searchText);

            _wildstarQuery = CreateWildcardBoolQuery(listOfWildCardedWords, fields);

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

        private static Func<QueryContainerDescriptor<T>, QueryContainer> CreateWildcardBoolQuery(
            List<string> words, List<string> fields, int? minimumShouldMatch = 1)
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> query =
                (containerDescriptor) => containerDescriptor.Bool(b => b
                    .Should(fields.Select(field =>
                        (QueryContainer) new BoolQuery
                        {
                            Should = words.Select(word =>
                                (QueryContainer) new WildcardQuery
                                {
                                    Value = word,
                                    Field = field
                                }).ToList(),
                            MinimumShouldMatch = words.Count
                        }).ToArray()
                    )
                );

            return query;
        }
        public QueryContainer Build(QueryContainerDescriptor<T> containerDescriptor)
        {
            var queryContainer = containerDescriptor.Bool(x => x.Should(_wildstarQuery, _exactQuery));

            if (_multipleFilterQueries.Any())
            {
                var listOfMultiples = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
                listOfMultiples.AddRange(_multipleFilterQueries);

                /*
                 * We have to match ALL filter values, ie. a Must clause.
                 */
                queryContainer = containerDescriptor.Bool(x =>
                    x.Must(
                        containerDescriptor.Bool(x => x.Must(listOfMultiples)),
                        queryContainer
                    )
                );
            }

            if (_filterQueries.Any())
            {
                /*
                 * Each field must be match, but it can match any one of
                 * of the filter values (a Should clause).
                 *
                 * In C# terms, the logic would look like:
                 * (field1 == filterValue1 || field1 == filterValue2)
                 *   && (field2 == filterValue3 || field2 == filterValue4) 
                 *   && ...
                 *
                 * This is suitable for fields like Asset Type or Tenure Type,
                 * where we want to return all matching assets from a set of types.
                 */
                queryContainer = containerDescriptor.Bool(
                    x => x.Must(
                        _filterQueries.Select(fq =>
                            containerDescriptor.Bool(y => y.Should(fq))
                        )
                        .Concat(new[] { queryContainer })
                        .ToArray()
                    )
                );
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
