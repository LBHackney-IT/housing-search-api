using Nest;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HousingSearchApi.V2.Gateways;

static class SearchOperations
{
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        Nested(string path, Func<QueryContainerDescriptor<object>, QueryContainer> func)
    {
        return should => should.Nested(n => n
            .Path(path)
            .Query(func)
        );
    }

    // Score for matching a single (best) field
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        MultiMatchBestFields(string searchText, Fields fields = null, int boost = 1) =>
        should => should
            .MultiMatch(mm => mm
                .Fields(fields ?? new[] { "*" })
                .Query(searchText)
                .Type(TextQueryType.BestFields)
                .Operator(Operator.And)
                .Boost(boost)
            );

    // Score for matching the combination of many fields
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        MultiMatchCrossFields(string searchText, Fields fields = null, int boost = 1) =>
        should => should
            .MultiMatch(mm => mm
                .Fields(fields ?? new[] { "*" })
                .Query(searchText)
                .Type(TextQueryType.CrossFields)
                .Operator(Operator.Or)
                .Boost(boost)
            );

    // Score for matching a high number (quantity) of fields
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        MultiMatchMostFields(string searchText, int boost, Fields fields = null) =>
        should => should
            .MultiMatch(mm => mm
                .Fields(fields ?? new[] { "*" })
                .Query(searchText)
                .Type(TextQueryType.MostFields)
                .Operator(Operator.Or)
                .Fuzziness(Fuzziness.Auto)
                .Boost(boost)
            );


    // Score for matching a value which contains the search text
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        WildcardMatch(string searchText, Fields fields, int boost)
    {
        List<string> ProcessWildcards(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                return new List<string>();
            return phrase.Split(' ').Select(word => $"*{word}*").ToList();
        }

        var listOfWildcardedWords = ProcessWildcards(searchText);
        var wildcardQueries = fields.SelectMany(fieldName =>
            listOfWildcardedWords.Select(term =>
                new WildcardQuery
                {
                    Field = fieldName,
                    Value = term,
                    Boost = boost
                }
                )
        ).ToList();

        return q => q.Bool(b => b
            .Should(wildcardQueries.Select(wq =>
                new QueryContainerDescriptor<object>()
                    .Wildcard(w => w
                        .Field(wq.Field)
                        .Value(wq.Value)
                        .Boost(wq.Boost)
                    )
                ).ToArray()
            )
        );
    }

    // basic match on field
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        MatchField(string searchText, Field field, int boost)
    {
        return should => should
            .Match(m => m
                .Field(field)
                .Query(searchText)
                .Fuzziness(Fuzziness.Auto)
                .Boost(boost)
            );
    }

    // basic match on a list of fields
    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        MatchFields(string searchText, Fields fields, int boost)
    {
        return q => q.Bool(b => b
            .Should(
                fields.Select(fieldName =>
                    MatchField(searchText, fieldName, boost)
                ).ToArray()
            )
        );
    }

    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        MatchPhrasePrefix(string searchText, Field field, int boost) =>
        should => should
            .MatchPhrasePrefix(mp => mp
                .Field(field)
                .Query(searchText)
                .Boost(boost)
                .Slop(1)
            );

    public static Func<QueryContainerDescriptor<object>, QueryContainer>
        MatchPhrasePrefixFields(string searchText, Fields fields, int boost)
    {
        return should => should.Bool(b => b
            .Should(
                fields.Select(fieldName =>
                    MatchPhrasePrefix(searchText, fieldName, boost)
                ).ToArray()
            )
        );
    }


    public static Func<QueryContainerDescriptor<object>, QueryContainer> QueryStringQuery(string searchText,
            List<string> fields, double? boost = null)
    {
        List<string> ProcessWildcards(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                return new List<string>();
            return phrase.Split(' ').Select(word => $"*{word}*").ToList();
        }

        var listOfWildCardedWords = ProcessWildcards(searchText);
        var queryString = $"({string.Join(" AND ", listOfWildCardedWords)}) " +
                          string.Join(' ', listOfWildCardedWords);

        Func<QueryContainerDescriptor<object>, QueryContainer> query =
            should => should.QueryString(q =>
            {
                var queryDescriptor = q.Query(queryString)
                    .Fields(f =>
                    {
                        foreach (var field in fields)
                        {
                            f = f.Field(field, boost);
                        }

                        return f;
                    });

                return queryDescriptor;
            });

        return query;
    }

}
